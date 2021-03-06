﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFeatureManager.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Declares the IFeatureManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Application
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Application.AttributedModel;
    using Kephas.Services;

    /// <summary>
    /// Shared service contract for managers of features within the application.
    /// </summary>
    /// <remarks>
    /// An application feature is a functional area of the application.
    /// It supports initialization and finalization.
    /// </remarks>
    [SharedAppServiceContract(AllowMultiple = true, MetadataAttributes = new[] { typeof(FeatureInfoAttribute) })]
    public interface IFeatureManager
    {
        /// <summary>
        /// Initializes the application feature asynchronously.
        /// </summary>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A Task.
        /// </returns>
        Task InitializeAsync(IAppContext appContext, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Finalizes the application feature asynchronously.
        /// </summary>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A Task.
        /// </returns>
        Task FinalizeAsync(IAppContext appContext, CancellationToken cancellationToken = default(CancellationToken));
    }
}