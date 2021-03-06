﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContextExtensions.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the data context extensions class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Data.Commands;
    using Kephas.Data.Linq;
    using Kephas.Data.Resources;
    using Kephas.Diagnostics.Contracts;
    using Kephas.Reflection;
    using Kephas.Threading.Tasks;

    /// <summary>
    /// Extension methods for <see cref="IDataContext"/>.
    /// </summary>
    public static class DataContextExtensions
    {
        /// <summary>
        /// The find asynchronous method.
        /// </summary>
        private static readonly MethodInfo FindAsyncMethod;

        /// <summary>
        /// The create command method.
        /// </summary>
        private static readonly MethodInfo CreateCommandMethod;

        /// <summary>
        /// The create entity asynchronous method.
        /// </summary>
        private static readonly MethodInfo CreateEntityAsyncMethod;

        /// <summary>
        /// Initializes static members of the <see cref="DataContextExtensions"/> class.
        /// </summary>
        static DataContextExtensions()
        {
            FindAsyncMethod = ReflectionHelper.GetGenericMethodOf(_ => DataContextExtensions.FindAsync<string>(null, null, CancellationToken.None));
            CreateEntityAsyncMethod = ReflectionHelper.GetGenericMethodOf(_ => DataContextExtensions.CreateEntityAsync<string>(null, null, CancellationToken.None));
            CreateCommandMethod = ReflectionHelper.GetGenericMethodOf(_ => ((IDataContext)null).CreateCommand<IDataCommand>());
        }

        /// <summary>
        /// Searches for the entity with the provided ID and returns it asynchronously.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="operationContext">Context for the create entity operation.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A promise of the found entity.
        /// </returns>
        public static async Task<object> CreateEntityAsync(
            this IDataContext dataContext,
            Type entityType,
            ICreateEntityContext operationContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Requires.NotNull(dataContext, nameof(dataContext));

            var createEntityAsync = CreateEntityAsyncMethod.MakeGenericMethod(entityType);
            var resultTask = (Task)createEntityAsync.Call(null, dataContext, operationContext, cancellationToken);
            await resultTask.PreserveThreadContext();

            var result = resultTask.GetPropertyValue(nameof(Task<int>.Result));
            return result;
        }

        /// <summary>
        /// Searches for the entity with the provided ID and returns it asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="dataContext">The data context.</param>
        /// <param name="operationContext">Context for the create entity operation.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A promise of the found entity.
        /// </returns>
        public static async Task<T> CreateEntityAsync<T>(
            this IDataContext dataContext,
            ICreateEntityContext operationContext,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            Requires.NotNull(dataContext, nameof(dataContext));

            var createCommand = CreateCommandMethod.MakeGenericMethod(typeof(ICreateEntityCommand));
            var command = (IDataCommand<ICreateEntityContext, ICreateEntityResult>)createCommand.Call(dataContext);
            var result = await command.ExecuteAsync(operationContext, cancellationToken).PreserveThreadContext();
            return (T)result.Entity;
        }

        /// <summary>
        /// Searches for the entity with the provided ID and returns it asynchronously.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="findContext">Context for the find operation.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A promise of the found entity.
        /// </returns>
        public static async Task<object> FindAsync(
            this IDataContext dataContext,
            Type entityType,
            IFindContext findContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Requires.NotNull(dataContext, nameof(dataContext));

            var findAsync = FindAsyncMethod.MakeGenericMethod(entityType);
            var resultTask = (Task)findAsync.Call(null, dataContext, findContext, cancellationToken);
            await resultTask.PreserveThreadContext();
            
            var result = resultTask.GetPropertyValue(nameof(Task<int>.Result));
            return result;
        }

        /// <summary>
        /// Searches for the entity with the provided ID and returns it asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="dataContext">The data context.</param>
        /// <param name="findContext">Context for the find operation.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A promise of the found entity.
        /// </returns>
        public static async Task<T> FindAsync<T>(
            this IDataContext dataContext,
            IFindContext findContext,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            Requires.NotNull(dataContext, nameof(dataContext));

            var createCommand = CreateCommandMethod.MakeGenericMethod(typeof(IFindCommand));
            var command = (IDataCommand<IFindContext, IFindResult>)createCommand.Call(dataContext);
            var result = await command.ExecuteAsync(findContext, cancellationToken).PreserveThreadContext();
            return (T)result.Entity;
        }

        /// <summary>
        /// Searches for the first entity matching the provided criteria and returns it asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="dataContext">The data context.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="throwIfNotFound"><c>true</c> to throw if not found (optional).</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A promise of the found entity.
        /// </returns>
        public static async Task<T> FindOneAsync<T>(
            this IDataContext dataContext, 
            Expression<Func<T, bool>> criteria,
            bool throwIfNotFound = true,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            Requires.NotNull(dataContext, nameof(dataContext));
            Requires.NotNull(criteria, nameof(criteria));

            var queryContext = new QueryOperationContext(dataContext);
            var query = dataContext.Query<T>(queryContext).Where(criteria).Take(2);
            var result = await query.ToListAsync(cancellationToken).PreserveThreadContext();
            if (result.Count > 1)
            {
                throw new AmbiguousMatchDataException(string.Format(Strings.DataContext_FindOneAsync_AmbiguousMatch_Exception, criteria));
            }

            if (result.Count == 0 && throwIfNotFound)
            {
                throw new NotFoundDataException(string.Format(Strings.DataContext_FindOneAsync_NotFound_Exception, criteria));
            }

            return result.Count == 0 ? default(T) : result[0];
        }

        /// <summary>
        /// Persists the changes in the dataContext asynchronously.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>
        /// A promise of the persist result.
        /// </returns>
        public static async Task<IDataCommandResult> PersistChangesAsync(
            this IDataContext dataContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Requires.NotNull(dataContext, nameof(dataContext));

            var createCommand = CreateCommandMethod.MakeGenericMethod(typeof(IPersistChangesCommand));
            var command = (IDataCommand<IPersistChangesContext, IDataCommandResult>)createCommand.Call(dataContext);
            var persistContext = new PersistChangesContext(dataContext);
            var result = await command.ExecuteAsync(persistContext, cancellationToken).PreserveThreadContext();
            return result;
        }

        /// <summary>
        /// Discards the changes in the data context.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public static void DiscardChanges(this IDataContext dataContext)
        {
            Requires.NotNull(dataContext, nameof(dataContext));

            var createCommand = CreateCommandMethod.MakeGenericMethod(typeof(IDiscardChangesCommand));
            var command = (IDataCommand<IDataOperationContext, IDataCommandResult>)createCommand.Call(dataContext);
            var persistContext = new DataOperationContext(dataContext);
            command.ExecuteAsync(persistContext).WaitNonLocking(TimeSpan.FromSeconds(1));
        }
    }
}