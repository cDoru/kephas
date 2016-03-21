﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MefCompositionContextBase.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the MEF composition context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Composition.Mef.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Diagnostics.Contracts;

    using Kephas.Composition.Mef.Internals;
    using Kephas.Composition.Mef.Resources;

    /// <summary>
    /// A MEF composition context.
    /// </summary>
    public abstract class MefCompositionContextBase : ICompositionContext
    {
        /// <summary>
        /// The inner container.
        /// </summary>
        private CompositionContext innerContainer;

        /// <summary>
        /// The scope provider.
        /// </summary>
        private MefScopeProvider scopeProvider;

        /// <summary>
        /// Resolves the specified contract type.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="contractName">The contract name.</param>
        /// <returns>An object implementing <paramref name="contractType"/>.</returns>
        public object GetExport(Type contractType, string contractName = null)
        {
            this.AssertNotDisposed();

            var component = string.IsNullOrEmpty(contractName)
                              ? this.innerContainer.GetExport(contractType)
                              : this.innerContainer.GetExport(contractType, contractName);
            return component;
        }

        /// <summary>
        /// Resolves the specified contract type returning multiple instances.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="contractName">The contract name.</param>
        /// <returns>An enumeration of objects implementing <paramref name="contractType"/>.</returns>
        public IEnumerable<object> GetExports(Type contractType, string contractName = null)
        {
            this.AssertNotDisposed();

            var components = string.IsNullOrEmpty(contractName)
                              ? this.innerContainer.GetExports(contractType)
                              : this.innerContainer.GetExports(contractType, contractName);
            return components;
        }

        /// <summary>
        /// Resolves the specified contract type.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="contractName">The contract name.</param>
        /// <returns>
        /// An object implementing <typeparamref name="T" />.
        /// </returns>
        public T GetExport<T>(string contractName = null)
        {
            this.AssertNotDisposed();

            var component = string.IsNullOrEmpty(contractName)
                              ? this.innerContainer.GetExport<T>()
                              : this.innerContainer.GetExport<T>(contractName);
            return component;
        }

        /// <summary>
        /// Resolves the specified contract type returning multiple instances.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="contractName">The contract name.</param>
        /// <returns>
        /// An enumeration of objects implementing <typeparamref name="T" />.
        /// </returns>
        public IEnumerable<T> GetExports<T>(string contractName = null)
        {
            this.AssertNotDisposed();

            var components = string.IsNullOrEmpty(contractName)
                              ? this.innerContainer.GetExports<T>()
                              : this.innerContainer.GetExports<T>(contractName);
            return components;
        }

        /// <summary>
        /// Tries to resolve the specified contract type.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="contractName">The contract name.</param>
        /// <returns>
        /// An object implementing <paramref name="contractType" />, or <c>null</c> if a service with the provided contract was not found.
        /// </returns>
        public object TryGetExport(Type contractType, string contractName = null)
        {
            this.AssertNotDisposed();

            object component;
            var successful = string.IsNullOrEmpty(contractName)
                              ? this.innerContainer.TryGetExport(contractType, out component)
                              : this.innerContainer.TryGetExport(contractType, contractName, out component);
            return successful ? component : null;
        }

        /// <summary>
        /// Tries to resolve the specified contract type.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="contractName">The contract name.</param>
        /// <returns>
        /// An object implementing <typeparamref name="T" />, or <c>null</c> if a service with the provided contract was not found.
        /// </returns>
        public T TryGetExport<T>(string contractName = null)
        {
            this.AssertNotDisposed();

            T component;
            var successful = string.IsNullOrEmpty(contractName)
                              ? this.innerContainer.TryGetExport(out component)
                              : this.innerContainer.TryGetExport(contractName, out component);
            return component;
        }

        /// <summary>
        /// Creates a new scoped composition context.
        /// </summary>
        /// <returns>
        /// The new scoped context.
        /// </returns>
        public virtual ICompositionContext CreateScopedContext()
        {
            this.scopeProvider = this.scopeProvider ?? this.GetExport<MefScopeProvider>();

            return new MefScopedCompositionContext(this.scopeProvider.CreateScopedContextExport());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            var disposableInnerContainer = this.innerContainer as IDisposable;
            disposableInnerContainer?.Dispose();

            this.innerContainer = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MefCompositionContextBase"/> class.
        /// </summary>
        /// <param name="context">The inner container.</param>
        protected void Initialize(CompositionContext context)
        {
            Contract.Requires(context != null);

            this.innerContainer = context;
        }

        /// <summary>
        /// Asserts that the container is not disposed.
        /// </summary>
        protected void AssertNotDisposed()
        {
            if (this.innerContainer == null)
            {
                throw new ObjectDisposedException(Strings.MefCompositionContainer_Disposed_Exception);
            }
        }
    }
}