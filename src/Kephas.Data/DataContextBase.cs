﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContextBase.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Base implementation of a data context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data
{
    using System;
    using System.Linq;

    using Kephas.Data.Capabilities;
    using Kephas.Data.Commands;
    using Kephas.Data.Commands.Factory;
    using Kephas.Data.Resources;
    using Kephas.Diagnostics.Contracts;
    using Kephas.Dynamic;
    using Kephas.Services;
    using Kephas.Services.Transitioning;

    /// <summary>
    /// Base implementation of a <see cref="IDataContext"/>.
    /// </summary>
    public abstract class DataContextBase : Expando, IDataContext
    {
        /// <summary>
        /// The initialization monitor.
        /// </summary>
        protected readonly InitializationMonitor<DataContextBase> InitializationMonitor;

        /// <summary>
        /// The data command provider.
        /// </summary>
        private readonly IDataCommandProvider dataCommandProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextBase"/> class.
        /// </summary>
        /// <param name="ambientServices">The ambient services.</param>
        /// <param name="dataCommandProvider">The data command provider.</param>
        protected DataContextBase(IAmbientServices ambientServices, IDataCommandProvider dataCommandProvider)
        {
            Requires.NotNull(ambientServices, nameof(ambientServices));
            Requires.NotNull(dataCommandProvider, nameof(dataCommandProvider));

            this.AmbientServices = ambientServices;
            this.dataCommandProvider = dataCommandProvider;
            this.Id = new Id(Guid.NewGuid());
            this.InitializationMonitor = new InitializationMonitor<DataContextBase>(this.GetType());
        }

        /// <summary>
        /// Gets the identifier for this instance.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Id Id { get; }

        /// <summary>
        /// Gets the ambient services.
        /// </summary>
        /// <value>
        /// The ambient services.
        /// </value>
        public IAmbientServices AmbientServices { get; }

        /// <summary>
        /// Initializes the service asynchronously.
        /// </summary>
        /// <param name="context">An optional context for initialization.</param>
        public virtual void Initialize(IContext context = null)
        {
            var config = context as IDataContextConfiguration;
            if (config == null)
            {
                throw new ArgumentException(string.Format(Strings.DataContextBase_BadInitializationContext_Exception, typeof(IDataContextConfiguration).FullName));
            }

            this.InitializationMonitor.Start();
            try
            {
                this.InitializeCore(config);
                this.InitializationMonitor.Complete();
            }
            catch (Exception ex)
            {
                this.InitializationMonitor.Fault(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets a query over the entity type for the given query operationContext, if any is provided.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="queryOperationContext">Context for the query.</param>
        /// <returns>
        /// A query over the entity type.
        /// </returns>
        public abstract IQueryable<T> Query<T>(IQueryOperationContext queryOperationContext = null)
            where T : class;

        /// <summary>
        /// Creates the command with the provided type.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command.</typeparam>
        /// <returns>
        /// The new command.
        /// </returns>
        public TCommand CreateCommand<TCommand>()
            where TCommand : IDataCommand
        {
            return (TCommand)this.dataCommandProvider.CreateCommand(this.GetType(), typeof(TCommand));
        }

        /// <summary>
        /// Tries to get a capability.
        /// </summary>
        /// <typeparam name="TCapability">Type of the capability.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="operationContext">Context for the operation (optional).</param>
        /// <returns>
        /// The capability.
        /// </returns>
        public virtual TCapability TryGetCapability<TCapability>(object entity, IDataOperationContext operationContext = null)
            where TCapability : class
        {
            var entityCapability = entity as TCapability;
            if (entityCapability != null)
            {
                return entityCapability;
            }

            return this.GetEntityInfo(entity) as TCapability;
        }

        /// <summary>
        /// Gets the entity extended information.
        /// </summary>
        /// <remarks>
        /// Note to inheritors: it is a good practice to override this method
        /// and provide a custom implementation, because, by default,
        /// the framework implementation returns each time a new <see cref="EntityInfo"/>.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The entity extended information.
        /// </returns>
        public virtual IEntityInfo GetEntityInfo(object entity)
        {
            Requires.NotNull(entity, nameof(entity));

            return this.CreateEntityInfo(entity);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Creates a new entity information.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="changeState">The entity's change state.</param>
        /// <returns>
        /// The new entity information.
        /// </returns>
        protected virtual IEntityInfo CreateEntityInfo(object entity, ChangeState changeState = ChangeState.NotChanged)
        {
            return new EntityInfo(entity, changeState);
        }

        /// <summary>
        /// Initializes the service asynchronously.
        /// </summary>
        /// <param name="config">The configuration for the data context.</param>
        protected virtual void InitializeCore(IDataContextConfiguration config)
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c>false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}