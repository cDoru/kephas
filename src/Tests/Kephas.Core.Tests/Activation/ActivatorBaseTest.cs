﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivatorBaseTest.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the activator base test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Core.Tests.Activation
{
    using System;

    using Kephas.Activation;
    using Kephas.Dynamic;
    using Kephas.Reflection;
    using Kephas.Runtime;
    using Kephas.Services;

    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="ActivatorBase"/>.
    /// </summary>
    [TestFixture]
    public class ActivatorBaseTest
    {
        [Test]
        public void CreateInstance_no_args()
        {
            var activator = new Activator();
            var date = activator.CreateInstance(typeof(DateTime).AsRuntimeTypeInfo());
            Assert.IsInstanceOf<DateTime>(date);
        }

        [Test]
        public void GetImplementationType_overridden_core()
        {
            var activator = new OverriddenActivator(t => typeof(string).AsRuntimeTypeInfo());
            var implementationType = activator.GetImplementationType(typeof(int).AsRuntimeTypeInfo());
            Assert.AreEqual(typeof(string), ((IRuntimeTypeInfo)implementationType).Type);
        }

        public class Activator : ActivatorBase { }

        public class OverriddenActivator : ActivatorBase
        {
            private readonly Func<ITypeInfo, ITypeInfo> converter;

            public OverriddenActivator(Func<ITypeInfo, ITypeInfo> converter)
            {
                this.converter = converter;
            }

            protected override ITypeInfo GetImplementationTypeCore(
                ITypeInfo abstractType,
                IContext activationContext = null,
                bool throwOnNotFound = true)
            {
                return this.converter(abstractType);
            }
        }
    }
}