﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncQueryProvider.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Contract for asynchronous query providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Linq
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract for asynchronous query providers.
    /// </summary>
    public interface IAsyncQueryProvider : IQueryProvider
    {
        /// <summary>
        /// Asynchronously executes the query represented by a specified expression tree.
        /// </summary>
        /// <param name="expression"> An expression tree that represents a LINQ query. </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the value that results from executing the specified query.
        /// </returns>
        Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously executes the strongly-typed query represented by a specified expression tree.
        /// </summary>
        /// <typeparam name="TResult"> The type of the value that results from executing the query. </typeparam>
        /// <param name="expression"> An expression tree that represents a LINQ query. </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the value that results from executing the specified query.
        /// </returns>
        Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default(CancellationToken));
    }
}