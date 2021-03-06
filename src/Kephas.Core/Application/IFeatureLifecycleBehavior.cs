﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFeatureLifecycleBehavior.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Declares the IFeatureLifecycleBehavior interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Application
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Application.Composition;
    using Kephas.Services;

    /// <summary>
    /// Shared service contract for feature lifecycle behaviors.
    /// </summary>
    /// <remarks>
    /// A feature lifecycle behavior intercepts the initialization and finalization of one or more features
    /// and reacts to them. Such features could log the performance of initialization,
    /// check prerequisites like proper licensing or whatever the application needs.
    /// </remarks>
    [SharedAppServiceContract(AllowMultiple = true)]
    public interface IFeatureLifecycleBehavior
    {
        /// <summary>
        /// Interceptor called before a feature starts its asynchronous initialization.
        /// </summary>
        /// <remarks>
        /// To interrupt the feature initialization, simply throw an appropriate exception.
        /// </remarks>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="serviceMetadata">The feature manager service metadata.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A Task.
        /// </returns>
        Task BeforeInitializeAsync(IAppContext appContext, FeatureManagerMetadata serviceMetadata, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Interceptor called after a feature completes its asynchronous initialization.
        /// </summary>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="serviceMetadata">The feature manager service metadata.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A Task.
        /// </returns>
        Task AfterInitializeAsync(IAppContext appContext, FeatureManagerMetadata serviceMetadata, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Interceptor called before a feature starts its asynchronous finalization.
        /// </summary>
        /// <remarks>
        /// To interrupt finalization, simply throw any appropriate exception.
        /// Caution! Interrupting the finalization may cause the application to remain in an undefined state.
        /// </remarks>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="serviceMetadata">The feature manager service metadata.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A Task.
        /// </returns>
        Task BeforeFinalizeAsync(IAppContext appContext, FeatureManagerMetadata serviceMetadata, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Interceptor called after a feature completes its asynchronous finalization.
        /// </summary>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="serviceMetadata">The feature manager service metadata.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A Task.
        /// </returns>
        Task AfterFinalizeAsync(IAppContext appContext, FeatureManagerMetadata serviceMetadata, CancellationToken cancellationToken = default(CancellationToken));
    }
}