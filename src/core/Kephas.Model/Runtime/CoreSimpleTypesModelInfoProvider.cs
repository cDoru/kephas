﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreSimpleTypesModelInfoProvider.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   A model information provider for simple types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Model.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Composition;
    using Kephas.Model.Elements.Construction;
    using Kephas.Model.Runtime.Construction.Builders;
    using Kephas.Model.Runtime.Factory;

    /// <summary>
    /// A model information provider for simple types.
    /// </summary>
    public class CoreSimpleTypesModelInfoProvider : RuntimeModelInfoProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreSimpleTypesModelInfoProvider"/> class.
        /// </summary>
        /// <param name="runtimeModelInfoFactory">  The runtime model information factory. </param>
        public CoreSimpleTypesModelInfoProvider(IRuntimeModelInfoFactory runtimeModelInfoFactory)
            : base(runtimeModelInfoFactory)
        {
        }

        /// <summary>
        /// Gets the element infos used for building the model space.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// An awaitable task promising an enumeration of element information.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed. Suppression is OK here.")]
        public override Task<IEnumerable<INamedElementInfo>> GetElementInfosAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var elementInfos = new List<INamedElementInfo>
                        {
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Boolean)).AsPrimitive().InCoreProjection().ElementInfo,

                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Byte)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.SByte)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Int16)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.UInt16)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Int32)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.UInt32)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Int64)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.UInt64)).AsPrimitive().InCoreProjection().ElementInfo,

                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Decimal)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Double)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Single)).AsPrimitive().InCoreProjection().ElementInfo,

                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.DateTime)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.DateTimeOffset)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.TimeSpan)).AsPrimitive().InCoreProjection().ElementInfo,

                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Uri)).AsPrimitive().InCoreProjection().ElementInfo,
                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.Guid)).AsPrimitive().InCoreProjection().ElementInfo,

                           new ValueTypeInfoBuilder(this.RuntimeModelInfoFactory, typeof(System.String)).AsPrimitive().InCoreProjection().ElementInfo,
                        };

            return Task.FromResult((IEnumerable<INamedElementInfo>)elementInfos);
        }
    }
}