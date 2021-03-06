﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultAppManagerTest.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the default application appManager test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Core.Tests.Application
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Application;
    using Kephas.Application.Composition;
    using Kephas.Composition;
    using Kephas.Services.Composition;
    using Kephas.Testing.Core.Composition;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public class DefaultAppManagerTest
    {
        [Test]
        public async Task InitializeAppAsync_right_featureManager_order_processingPriority()
        {
            var order = new List<int>();

            var featureManager1 = Substitute.For<IFeatureManager>();
            featureManager1.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(1));

            var featureManager2 = Substitute.For<IFeatureManager>();
            featureManager2.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(2));

            var appManager = new DefaultAppManager(
                Substitute.For<IAppManifest>(),
                Substitute.For<IAmbientServices>(),
                new List<IExportFactory<IAppLifecycleBehavior, AppServiceMetadata>>(),
                new[]
                    {
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager1, new FeatureManagerMetadata(processingPriority: 2)),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager2, new FeatureManagerMetadata(processingPriority: 1)),
                    },
                null);

            await appManager.InitializeAppAsync(Substitute.For<IAppContext>(), CancellationToken.None);

            Assert.AreEqual(2, order.Count);
            Assert.AreEqual(2, order[0]);
            Assert.AreEqual(1, order[1]);
        }

        [Test]
        public async Task InitializeAppAsync_right_featureManager_order_dependencies()
        {
            var order = new List<int>();

            var featureManager1 = Substitute.For<IFeatureManager>();
            featureManager1.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(1));

            var featureManager2 = Substitute.For<IFeatureManager>();
            featureManager2.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(2));

            var featureManager3 = Substitute.For<IFeatureManager>();
            featureManager3.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(3));

            var appManager = new DefaultAppManager(
                Substitute.For<IAppManifest>(),
                Substitute.For<IAmbientServices>(),
                new List<IExportFactory<IAppLifecycleBehavior, AppServiceMetadata>>(),
                new[]
                    {
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager1, new FeatureManagerMetadata(new FeatureInfo("1", new[] { "3" }))),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager2, new FeatureManagerMetadata(new FeatureInfo("2", new[] { "3" }))),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager3, new FeatureManagerMetadata(new FeatureInfo("3"))),
                    },
                null);

            await appManager.InitializeAppAsync(Substitute.For<IAppContext>(), CancellationToken.None);

            Assert.AreEqual(3, order.Count);
            Assert.AreEqual(3, order[0]);
            Assert.AreEqual(1, order[1]);
            Assert.AreEqual(2, order[2]);
        }

        [Test]
        public async Task InitializeAppAsync_right_behavior_order()
        {
            var order = new List<string>();

            var featureManager1 = Substitute.For<IFeatureManager>();
            featureManager1.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0));

            var featureManager2 = Substitute.For<IFeatureManager>();
            featureManager2.InitializeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0));

            var behavior1 = Substitute.For<IFeatureLifecycleBehavior>();
            behavior1.BeforeInitializeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("Before1"));
            behavior1.AfterInitializeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("After1"));

            var behavior2 = Substitute.For<IFeatureLifecycleBehavior>();
            behavior2.BeforeInitializeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("Before2"));
            behavior2.AfterInitializeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("After2"));

            var appManager = new DefaultAppManager(
                Substitute.For<IAppManifest>(),
                Substitute.For<IAmbientServices>(),
                new List<IExportFactory<IAppLifecycleBehavior, AppServiceMetadata>>(),
                new[]
                    {
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager1, new FeatureManagerMetadata()),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager2, new FeatureManagerMetadata()),
                    },
                new[]
                    {
                        new TestExportFactory<IFeatureLifecycleBehavior, AppServiceMetadata>(() => behavior1, new AppServiceMetadata(processingPriority: 2)),
                        new TestExportFactory<IFeatureLifecycleBehavior, AppServiceMetadata>(() => behavior2, new AppServiceMetadata(processingPriority: 1)),
                    });

            await appManager.InitializeAppAsync(Substitute.For<IAppContext>(), CancellationToken.None);

            Assert.AreEqual(8, order.Count);
            Assert.AreEqual("Before2", order[0]);
            Assert.AreEqual("Before1", order[1]);
            Assert.AreEqual("After1", order[2]);
            Assert.AreEqual("After2", order[3]);
            Assert.AreEqual("Before2", order[4]);
            Assert.AreEqual("Before1", order[5]);
            Assert.AreEqual("After1", order[6]);
            Assert.AreEqual("After2", order[7]);
        }

        [Test]
        public async Task FinalizeAppAsync_right_featureManager_order_processingPriority()
        {
            var order = new List<int>();

            var finalizer1 = Substitute.For<IFeatureManager>();
            finalizer1.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(1));

            var finalizer2 = Substitute.For<IFeatureManager>();
            finalizer2.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(2));

            var appManager = new DefaultAppManager(
                Substitute.For<IAppManifest>(),
                Substitute.For<IAmbientServices>(),
                new List<IExportFactory<IAppLifecycleBehavior, AppServiceMetadata>>(),
                new[]
                    {
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => finalizer1, new FeatureManagerMetadata(processingPriority: 2)),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => finalizer2, new FeatureManagerMetadata(processingPriority: 1)),
                    },
                null);

            await appManager.FinalizeAppAsync(Substitute.For<IAppContext>(), CancellationToken.None);

            Assert.AreEqual(2, order.Count);
            Assert.AreEqual(1, order[0]);
            Assert.AreEqual(2, order[1]);
        }

        [Test]
        public async Task FinalizeAppAsync_right_featureManager_order_dependencies()
        {
            var order = new List<int>();

            var finalizer1 = Substitute.For<IFeatureManager>();
            finalizer1.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(1));

            var finalizer2 = Substitute.For<IFeatureManager>();
            finalizer2.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(2));

            var finalizer3 = Substitute.For<IFeatureManager>();
            finalizer3.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add(3));

            var appManager = new DefaultAppManager(
                Substitute.For<IAppManifest>(),
                Substitute.For<IAmbientServices>(),
                new List<IExportFactory<IAppLifecycleBehavior, AppServiceMetadata>>(),
                new[]
                    {
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => finalizer1, new FeatureManagerMetadata(new FeatureInfo("1", new[] { "3" }))),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => finalizer2, new FeatureManagerMetadata(new FeatureInfo("2", new[] { "1" }))),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => finalizer3, new FeatureManagerMetadata(new FeatureInfo("3"))),
                    },
                null);

            await appManager.FinalizeAppAsync(Substitute.For<IAppContext>(), CancellationToken.None);

            Assert.AreEqual(3, order.Count);
            Assert.AreEqual(2, order[0]);
            Assert.AreEqual(1, order[1]);
            Assert.AreEqual(3, order[2]);
        }

        [Test]
        public async Task FinalizeAppAsync_right_behavior_order()
        {
            var order = new List<string>();

            var featureManager1 = Substitute.For<IFeatureManager>();
            featureManager1.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0));

            var featureManager2 = Substitute.For<IFeatureManager>();
            featureManager2.FinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0));

            var behavior1 = Substitute.For<IFeatureLifecycleBehavior>();
            behavior1.BeforeFinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("Before1"));
            behavior1.AfterFinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("After1"));

            var behavior2 = Substitute.For<IFeatureLifecycleBehavior>();
            behavior2.BeforeFinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("Before2"));
            behavior2.AfterFinalizeAsync(Arg.Any<IAppContext>(), Arg.Any<FeatureManagerMetadata>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(0))
                .AndDoes(_ => order.Add("After2"));

            var appManager = new DefaultAppManager(
                Substitute.For<IAppManifest>(),
                Substitute.For<IAmbientServices>(),
                new List<IExportFactory<IAppLifecycleBehavior, AppServiceMetadata>>(),
                new[]
                    {
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager1, new FeatureManagerMetadata()),
                        new TestExportFactory<IFeatureManager, FeatureManagerMetadata>(() => featureManager2, new FeatureManagerMetadata()),
                    },
                new[]
                    {
                        new TestExportFactory<IFeatureLifecycleBehavior, AppServiceMetadata>(() => behavior1, new AppServiceMetadata(processingPriority: 2)),
                        new TestExportFactory<IFeatureLifecycleBehavior, AppServiceMetadata>(() => behavior2, new AppServiceMetadata(processingPriority: 1)),
                    });

            await appManager.FinalizeAppAsync(Substitute.For<IAppContext>(), CancellationToken.None);

            Assert.AreEqual(8, order.Count);
            Assert.AreEqual("Before1", order[0]);
            Assert.AreEqual("Before2", order[1]);
            Assert.AreEqual("After2", order[2]);
            Assert.AreEqual("After1", order[3]);
            Assert.AreEqual("Before1", order[4]);
            Assert.AreEqual("Before2", order[5]);
            Assert.AreEqual("After2", order[6]);
            Assert.AreEqual("After1", order[7]);
        }
    }
}