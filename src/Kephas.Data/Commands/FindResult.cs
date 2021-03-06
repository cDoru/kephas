﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindResult.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the find result class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Commands
{
    using System;

    /// <summary>
    /// Encapsulates the result of a find.
    /// </summary>
    public class FindResult : DataCommandResult, IFindResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindResult"/> class.
        /// </summary>
        /// <param name="entity">The found entity.</param>
        /// <param name="message">The message (optional).</param>
        /// <param name="exception">The exception (optional).</param>
        public FindResult(object entity, string message = null, Exception exception = null)
            : base(message, exception)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Gets the found entity or <c>null</c> if no entity could be found.
        /// </summary>
        /// <value>
        /// The found entity.
        /// </value>
        public object Entity { get; }
    }
}