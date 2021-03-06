﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionConverterMetadata.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the expression converter metadata class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Client.Queries.Conversion.Composition
{
    using System.Collections.Generic;

    using Kephas.Collections;
    using Kephas.Diagnostics.Contracts;
    using Kephas.Services.Composition;

    /// <summary>
    /// An expression converter metadata.
    /// </summary>
    public class ExpressionConverterMetadata : AppServiceMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionConverterMetadata"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public ExpressionConverterMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata == null)
            {
                return;
            }

            this.Operator = (string)metadata.TryGetValue(nameof(this.Operator));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionConverterMetadata"/> class.
        /// </summary>
        /// <param name="operator">The operator.</param>
        /// <param name="processingPriority">The processing priority (optional).</param>
        /// <param name="overridePriority">The override priority (optional).</param>
        /// <param name="optionalService">True to optional service (optional).</param>
        public ExpressionConverterMetadata(string @operator, int processingPriority = 0, int overridePriority = 0, bool optionalService = false)
            : base(processingPriority, overridePriority, optionalService)
        {
            Requires.NotNullOrEmpty(@operator, nameof(@operator));

            this.Operator = @operator;
        }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public string Operator { get; }
    }
}