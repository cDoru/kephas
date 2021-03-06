﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceEnumerableExtensionsTest.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the service enumerable extensions test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Core.Tests.Services.Behavior
{
    using System.Collections.Generic;
    using System.Linq;

    using Kephas.Behavior;
    using Kephas.Composition;
    using Kephas.Services.Behavior;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public class ServiceEnumerableExtensionsTest
    {
        [Test]
        public void WhereEnabled_no_behaviors()
        {
            var ambientServicesMock = this.CreateAmbientServicesMock();

            var services = new List<ITestService> { Substitute.For<ITestService>() };
            var filteredServices = services.WhereEnabled(ambientServicesMock).ToList();
            Assert.AreEqual(services.Count, filteredServices.Count);
            Assert.AreEqual(services[0], filteredServices[0]);
        }

        [Test]
        public void WhereEnabled_exclude_all_behaviors()
        {
            var excludeAllBehaviorMock = this.CreateEnabledServiceBehaviorRule(canApply: true, isEndRule: false, value: false);
            var ambientServicesMock = this.CreateAmbientServicesMock(excludeAllBehaviorMock);

            var services = new List<ITestService> { Substitute.For<ITestService>() };
            var filteredServices = services.WhereEnabled(ambientServicesMock).ToList();
            Assert.AreEqual(0, filteredServices.Count);
        }

        [Test]
        public void WhereEnabled_priority_with_end_rule()
        {
            var excludeBehaviorMock = this.CreateEnabledServiceBehaviorRule(canApply: true, isEndRule: false, value: false, processingPriority: 1);
            var includeBehaviorMock = this.CreateEnabledServiceBehaviorRule(canApply: true, isEndRule: true, value: true, processingPriority: 0);
            var ambientServicesMock = this.CreateAmbientServicesMock(excludeBehaviorMock, includeBehaviorMock);

            var services = new List<ITestService> { Substitute.For<ITestService>() };
            var filteredServices = services.WhereEnabled(ambientServicesMock).ToList();
            Assert.AreEqual(services.Count, filteredServices.Count);
            Assert.AreEqual(services[0], filteredServices[0]);
        }

        [Test]
        public void WhereEnabled_priority_without_end_rule()
        {
            var excludeBehaviorMock = this.CreateEnabledServiceBehaviorRule(canApply: true, isEndRule: false, value: false, processingPriority: 1);
            var includeBehaviorMock = this.CreateEnabledServiceBehaviorRule(canApply: true, isEndRule: false, value: true, processingPriority: 0);
            var ambientServicesMock = this.CreateAmbientServicesMock(excludeBehaviorMock, includeBehaviorMock);

            var services = new List<ITestService> { Substitute.For<ITestService>() };
            var filteredServices = services.WhereEnabled(ambientServicesMock).ToList();
            Assert.AreEqual(0, filteredServices.Count);
        }

        private IAmbientServices CreateAmbientServicesMock(params IEnabledServiceBehaviorRule<ITestService>[] rules)
        {
            var compositionContextMock = Substitute.For<ICompositionContext>();
            compositionContextMock.GetExports<IEnabledServiceBehaviorRule<ITestService>>(Arg.Any<string>())
                .Returns(new List<IEnabledServiceBehaviorRule<ITestService>>(rules));

            var ambientServicesMock = Substitute.For<IAmbientServices>();
            ambientServicesMock.CompositionContainer.Returns(compositionContextMock);
            return ambientServicesMock;
        }

        private IEnabledServiceBehaviorRule<ITestService> CreateEnabledServiceBehaviorRule(bool canApply, bool isEndRule, bool value, int processingPriority = 0)
        {
            var behaviorMock = Substitute.For<IEnabledServiceBehaviorRule<ITestService>>();
            behaviorMock.CanApply(Arg.Any<IServiceBehaviorContext<ITestService>>()).Returns(canApply);
            behaviorMock.IsEndRule.Returns(isEndRule);
            behaviorMock.ProcessingPriority.Returns(processingPriority);
            behaviorMock.GetValue(Arg.Any<IServiceBehaviorContext<ITestService>>())
                .Returns(value ? BehaviorValue.True : BehaviorValue.False);
            return behaviorMock;
        }

        public interface ITestService { }
    }
}