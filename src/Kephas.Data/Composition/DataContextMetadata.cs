﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContextMetadata.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the data context metadata class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Composition
{
    using System.Collections.Generic;

    using Kephas.Data.Store;
    using Kephas.Services.Composition;

    /// <summary>
    /// Metadata for <see cref="IDataContext"/> services.
    /// </summary>
    public class DataContextMetadata : AppServiceMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextMetadata"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public DataContextMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata == null)
            {
                return;
            }

            this.SupportedDataStoreKinds = this.GetMetadataValue<SupportedDataStoreKindsAttribute, IEnumerable<string>>(metadata, new string[0]);
        }

        /// <summary>
        /// Gets the kind of the supported data store.
        /// </summary>
        /// <value>
        /// The data store.
        /// </value>
        public IEnumerable<string> SupportedDataStoreKinds { get; }
    }
}