﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportMetadataBase.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Base class for export metadata.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Composition.Metadata
{
    using System.Collections.Generic;

    using Kephas.Dynamic;

    /// <summary>
    /// Base class for export metadata.
    /// </summary>
    public abstract class ExportMetadataBase : Expando
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportMetadataBase"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        protected ExportMetadataBase(IDictionary<string, object> metadata)
            : base(metadata ?? new Dictionary<string, object>())
        {
        }
    }
}