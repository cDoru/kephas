﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorTestBase.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the constructor test base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Model.Tests.Runtime.Construction
{
    using System;
    using System.Collections.Generic;

    using Kephas.Composition;
    using Kephas.Model.Construction;
    using Kephas.Model.Elements;
    using Kephas.Model.Runtime.Configuration;
    using Kephas.Model.Runtime.Configuration.Composition;
    using Kephas.Model.Runtime.Construction;
    using Kephas.Model.Runtime.Construction.Annotations;
    using Kephas.Model.Runtime.Construction.Composition;
    using Kephas.Runtime;
    using Kephas.Testing.Core.Composition;

    using NSubstitute;

    public class ConstructorTestBase
    {
        /// <summary>
        /// Gets the construction context.
        /// </summary>
        /// <param name="modelSpace">The model space.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>
        /// The construction context.
        /// </returns>
        public IModelConstructionContext GetConstructionContext(
            IModelSpace modelSpace = null,
            IRuntimeModelElementFactory factory = null)
        {
            return new ModelConstructionContext(Substitute.For<IAmbientServices>())
            {
                           ModelSpace = modelSpace ?? Substitute.For<IModelSpace>(),
                           RuntimeModelElementFactory = factory ?? this.GetNullRuntimeModelElementFactory(),
                       };
        }

        public IRuntimeModelElementFactory GetTestRuntimeModelElementFactory()
        {
            var constructors = new List<IExportFactory<IRuntimeModelElementConstructor, RuntimeModelElementConstructorMetadata>>
                                   {
                                       new TestExportFactory<IRuntimeModelElementConstructor, RuntimeModelElementConstructorMetadata>(() => new AnnotationConstructor(), new RuntimeModelElementConstructorMetadata(typeof(Annotation), typeof(IAnnotation), typeof(Attribute))),
                                       new TestExportFactory<IRuntimeModelElementConstructor, RuntimeModelElementConstructorMetadata>(() => new PropertyConstructor(), new RuntimeModelElementConstructorMetadata(typeof(Property), typeof(IProperty), typeof(IRuntimePropertyInfo))),
                                   };

            var configurators = new List<IExportFactory<IRuntimeModelElementConfigurator, RuntimeModelElementConfiguratorMetadata>>();

            var factory = new DefaultRuntimeModelElementFactory(constructors, configurators);
            return factory;
        }

        public IRuntimeModelElementFactory GetNullRuntimeModelElementFactory()
        {
            var factory = Substitute.For<IRuntimeModelElementFactory>();
            factory.TryCreateModelElement(Arg.Any<IModelConstructionContext>(), Arg.Any<object>())
                .Returns((INamedElement)null);
            return factory;
        }
    }
}