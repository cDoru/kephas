﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataValidationResult.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Declares the IDataValidationResult interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Validation
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// Interface for validation result.
    /// </summary>
    public interface IDataValidationResult : IEnumerable<IDataValidationResultItem>
    {
    }

    /// <summary>
    /// Extension methods for <see cref="IDataValidationResult"/>
    /// </summary>
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the validation result has errors.
        /// </summary>
        /// <param name="result">The validation result.</param>
        /// <returns>
        /// <c>true</c> if the validation result contains errors, <c>false</c> if not.
        /// </returns>
        public static bool HasErrors(this IDataValidationResult result)
        {
            Requires.NotNull(result, nameof(result));

            return result.Any(i => i.Severity == DataValidationSeverity.Error);
        }

        /// <summary>
        /// Gets only the errors items from this validation result.
        /// </summary>
        /// <param name="result">The validation result.</param>
        /// <returns>
        /// An enumration of errors.
        /// </returns>
        public static IEnumerable<IDataValidationResultItem> GetErrors(this IDataValidationResult result)
        {
            Requires.NotNull(result, nameof(result));

            return result.Where(i => i.Severity == DataValidationSeverity.Error);
        }
    }
}