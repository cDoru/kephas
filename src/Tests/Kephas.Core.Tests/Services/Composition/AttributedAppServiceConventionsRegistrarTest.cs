﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributedAppServiceConventionsRegistrarTest.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Test class for <see cref="AttributedAppServiceConventionsRegistrar" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Core.Tests.Services.Composition
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Kephas.Composition;
    using Kephas.Core.Tests.Composition;
    using Kephas.Services;
    using Kephas.Services.Composition;

    using NUnit.Framework;
    using Kephas.Testing.Core.Services.Composition;

    /// <summary>
    /// Test class for <see cref="AttributedAppServiceConventionsRegistrar"/>.
    /// </summary>
    [TestFixture]
    public class AttributedAppServiceConventionsRegistrarTest
    {
        [Test]
        public void RegisterConventions_Multiple()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(IMultipleTestAppService).GetTypeInfo(), 
                        typeof(MultipleTestService).GetTypeInfo(),
                        typeof(NewMultipleTestService).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            Assert.AreEqual(1, conventions.MatchingConventionsBuilders.Count);
            var builderEntry = conventions.MatchingConventionsBuilders.Single();

            Assert.IsTrue(builderEntry.Key(typeof(MultipleTestService)));
            Assert.IsTrue(builderEntry.Key(typeof(NewMultipleTestService)));
        }

        [Test]
        public void RegisterConventions_Single_one_service()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(ISingleTestAppService).GetTypeInfo(), 
                        typeof(SingleTestService).GetTypeInfo()
                    },
                new TestRegistrationContext());

            Assert.AreEqual(1, conventions.TypeConventionsBuilders.Count);
            Assert.IsTrue(conventions.TypeConventionsBuilders.ContainsKey(typeof(SingleTestService)));
        }

        [Test]
        public void RegisterConventions_Single_override_service_success()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(ISingleTestAppService).GetTypeInfo(), 
                        typeof(SingleTestService).GetTypeInfo(),
                        typeof(SingleOverrideTestService).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            Assert.AreEqual(1, conventions.TypeConventionsBuilders.Count);
            Assert.IsTrue(conventions.TypeConventionsBuilders.ContainsKey(typeof(SingleOverrideTestService)));
        }

        [Test]
        public void RegisterConventions_Single_override_service_failure()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            Assert.Throws<InvalidOperationException>(() => registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(ISingleTestAppService).GetTypeInfo(), 
                        typeof(SingleTestService).GetTypeInfo(),
                        typeof(SingleSameOverrideTestService).GetTypeInfo(),
                    },
                new TestRegistrationContext()));
        }

        [Test]
        public void RegisterConventions_generic()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(IGenericAppService<>).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            Assert.AreEqual(1, conventions.MatchingConventionsBuilders.Count);
            var match = conventions.MatchingConventionsBuilders.Keys.First();
            Assert.IsTrue(match(typeof(GenericAppService<>)));
            Assert.IsFalse(match(typeof(TwoGenericAppServiceIntBool)));
        }

        [Test]
        public void RegisterConventions_default_metadata()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(ISimpleMetadataAppService).GetTypeInfo(),
                        typeof(SimpleMetadataAppService).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            var testBuilder = (CompositionContainerBuilderBaseTest.TestPartConventionsBuilder)conventions.TypeConventionsBuilders[typeof(SimpleMetadataAppService)];
            var metadata = testBuilder.ExportBuilder.Metadata;

            Assert.AreEqual(4, metadata.Count);
            Assert.IsTrue(metadata.ContainsKey("ProcessingPriority"));
            Assert.IsTrue(metadata.ContainsKey("OverridePriority"));
            Assert.IsTrue(metadata.ContainsKey("OptionalService"));
            Assert.IsTrue(metadata.ContainsKey(nameof(AppServiceMetadata.AppServiceImplementationType)));

            var valueGetter = (Func<Type, object>)metadata[nameof(AppServiceMetadata.AppServiceImplementationType)];
            Assert.AreEqual(typeof(ISimpleMetadataAppService), valueGetter(typeof(ISimpleMetadataAppService)));
            Assert.AreEqual(null, valueGetter(null));

            valueGetter = (Func<Type, object>)metadata["ProcessingPriority"];
            Assert.AreEqual(null, valueGetter(typeof(ISimpleMetadataAppService)));

            valueGetter = (Func<Type, object>)metadata["OverridePriority"];
            Assert.AreEqual(null, valueGetter(typeof(ISimpleMetadataAppService)));

            valueGetter = (Func<Type, object>)metadata["OptionalService"];
            Assert.AreEqual(null, valueGetter(typeof(ISimpleMetadataAppService)));
        }

        [Test]
        public void RegisterConventions_generic_with_nongeneric_base()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(IOneGenericAppService<>).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            Assert.AreEqual(1, conventions.MatchingConventionsBuilders.Count);
            var testBuilder = (CompositionContainerBuilderBaseTest.TestPartConventionsBuilder)conventions.MatchingConventionsBuilders.Values.First();
            Assert.AreEqual(typeof(IOneGenericAppService), testBuilder.ExportBuilder.ContractType);
        }

        [Test]
        public void RegisterConventions_generic_with_nongeneric_metadata()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(IOneGenericAppService<>).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            var testBuilder = (CompositionContainerBuilderBaseTest.TestPartConventionsBuilder)conventions.MatchingConventionsBuilders.Values.First();
            var metadata = testBuilder.ExportBuilder.Metadata;

            Assert.AreEqual(5, metadata.Count);
            Assert.IsTrue(metadata.ContainsKey("TType"));

            var valueGetter = (Func<Type, object>)metadata["TType"];
            Assert.AreEqual(typeof(string), valueGetter(typeof(OneGenericAppServiceString)));
        }

        [Test]
        public void RegisterConventions_generic_with_nongeneric_metadata_two()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(ITwoGenericAppService<,>).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            var testBuilder = (CompositionContainerBuilderBaseTest.TestPartConventionsBuilder)conventions.MatchingConventionsBuilders.Values.First();
            var metadata = testBuilder.ExportBuilder.Metadata;

            Assert.AreEqual(6, metadata.Count);
            Assert.IsTrue(metadata.ContainsKey("FromType"));
            Assert.IsTrue(metadata.ContainsKey("ToType"));

            var valueGetter = (Func<Type, object>)metadata["FromType"];
            Assert.AreEqual(typeof(int), valueGetter(typeof(TwoGenericAppServiceIntBool)));

            valueGetter = (Func<Type, object>)metadata["ToType"];
            Assert.AreEqual(typeof(bool), valueGetter(typeof(TwoGenericAppServiceIntBool)));
        }

        [Test]
        public void RegisterConventions_metadata()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            registrar.RegisterConventions(
                conventions,
                new[]
                    {
                        typeof(IMetadataAppService).GetTypeInfo(),
                        typeof(MetadataAppService).GetTypeInfo(),
                        typeof(NullMetadataAppService).GetTypeInfo(),
                    },
                new TestRegistrationContext());

            Assert.AreEqual(1, conventions.MatchingConventionsBuilders.Count);
            var builderEntry = conventions.MatchingConventionsBuilders.First();
            Assert.IsTrue(builderEntry.Key(typeof(NullMetadataAppService)));

            var testBuilder = (CompositionContainerBuilderBaseTest.TestPartConventionsBuilder)builderEntry.Value;
            var metadata = testBuilder.ExportBuilder.Metadata;

            Assert.AreEqual(4, metadata.Count);
            Assert.IsTrue(metadata.ContainsKey("ProcessingPriority"));

            var valueGetter = (Func<Type, object>)metadata["ProcessingPriority"];
            Assert.AreEqual(100, valueGetter(typeof(MetadataAppService)));
            Assert.IsNull(valueGetter(typeof(NullMetadataAppService)));
        }

        [Test]
        public void RegisterConventions_bad_contract_type()
        {
            var conventions = new CompositionContainerBuilderBaseTest.TestConventionsBuilder();

            var registrar = new AttributedAppServiceConventionsRegistrar();
            Assert.Throws<CompositionException>(
                () => registrar.RegisterConventions(
                    conventions,
                    new[] {
                            typeof(IBadAppService).GetTypeInfo(), 
                            typeof(BadAppService).GetTypeInfo(),
                    },
                    new TestRegistrationContext()));
        }

        [SharedAppServiceContract(AllowMultiple = false)]
        public interface ISingleTestAppService { }

        [SharedAppServiceContract(AllowMultiple = true)]
        public interface IMultipleTestAppService { }

        public class SingleTestService : ISingleTestAppService { }

        [OverridePriority(Priority.High)]
        public class SingleOverrideTestService : ISingleTestAppService { }

        public class SingleSameOverrideTestService : ISingleTestAppService { }

        public class MultipleTestService : IMultipleTestAppService { }

        public class NewMultipleTestService : IMultipleTestAppService { }


        [AppServiceContract]
        public interface ISimpleMetadataAppService { }

        public class SimpleMetadataAppService : ISimpleMetadataAppService
        {
        }

        [SharedAppServiceContract(AllowMultiple = true, MetadataAttributes = new[] { typeof(ProcessingPriorityAttribute) })]
        public interface IMetadataAppService { }

        [ProcessingPriority(100)]
        public class MetadataAppService : IMetadataAppService { }

        public class NullMetadataAppService : IMetadataAppService { }


        [SharedAppServiceContract]
        public interface IGenericAppService<T> { }

        public interface IOneGenericAppService { }

        [SharedAppServiceContract(ContractType = typeof(IOneGenericAppService))]
        public interface IOneGenericAppService<T> : IOneGenericAppService { }

        public interface ITwoGenericAppService { }

        [SharedAppServiceContract(ContractType = typeof(ITwoGenericAppService))]
        public interface ITwoGenericAppService<TFrom, ToType> : ITwoGenericAppService { }

        public class GenericAppService<T> : IGenericAppService<T> { }

        public class OneGenericAppServiceString : IOneGenericAppService<string> { }

        public class TwoGenericAppServiceIntBool : ITwoGenericAppService<int, bool> { }

        [SharedAppServiceContract(ContractType = typeof(IDisposable))]
        public interface IBadAppService { }

        public class BadAppService : IBadAppService { }
    }
}