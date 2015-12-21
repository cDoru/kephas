﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuntimeModelDimensionInfoFactory.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Runtime provider for model dimension information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Model.Runtime.Factory
{
    using System.Reflection;

    using Kephas.Model.AttributedModel;
    using Kephas.Model.Runtime.Construction;
    using Kephas.Services;

    /// <summary>
    /// Runtime factory for model dimension information.
    /// </summary>
    [ProcessingPriority(Priority.Highest)]
    public class RuntimeModelDimensionInfoFactory : RuntimeModelElementInfoFactoryBase<RuntimeModelDimensionInfo, TypeInfo>
    {
        /// <summary>
        /// Tries to get the model dimension information.
        /// </summary>
        /// <param name="runtimeElementInfoFactoryDispatcher">The runtime model information provider.</param>
        /// <param name="runtimeElement">The runtime element.</param>
        /// <returns>
        /// A new element information based on the provided runtime element information, or <c>null</c>
        /// if the runtime element information is not supported.
        /// </returns>
        protected override RuntimeModelDimensionInfo TryGetElementInfoCore(IRuntimeElementInfoFactoryDispatcher runtimeElementInfoFactoryDispatcher, TypeInfo runtimeElement)
        {
            if (!runtimeElement.IsInterface)
            {
                return null;
            }

            var dimensionAttribute = runtimeElement.GetCustomAttribute<ModelDimensionAttribute>();
            if (dimensionAttribute == null)
            {
                return null;
            }

            return new RuntimeModelDimensionInfo(runtimeElement);
        }
    }
}