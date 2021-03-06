﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityConstructor.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the entity constructor class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Model.Runtime.Construction
{
    using Kephas.Data.Model.Elements;
    using Kephas.Model.Construction;
    using Kephas.Model.Runtime.Construction;
    using Kephas.Runtime;

    /// <summary>
    /// Classifier constructor for <see cref="Entity"/>.
    /// </summary>
    public class EntityConstructor : ClassifierConstructorBase<Entity, IEntity>
    {
        /// <summary>
        /// The entity discriminator.
        /// </summary>
        public const string EntityDiscriminator = "Entity";

        /// <summary>
        /// Gets the element name discriminator.
        /// </summary>
        /// <value>
        /// The element name discriminator.
        /// </value>
        /// <remarks>
        /// This discriminator can be used as a suffix in the name to identify the element type.
        /// </remarks>
        protected override string ElementNameDiscriminator => EntityDiscriminator;

        /// <summary>
        /// Core implementation of trying to get the element information.
        /// </summary>
        /// <param name="constructionContext">Context for the construction.</param>
        /// <param name="runtimeElement">The runtime element.</param>
        /// <returns>
        /// A new element information based on the provided runtime element information, or <c>null</c>
        /// if the runtime element information is not supported.
        /// </returns>
        protected override Entity TryCreateModelElementCore(IModelConstructionContext constructionContext, IRuntimeTypeInfo runtimeElement)
        {
            return new Entity(constructionContext, this.TryComputeName(constructionContext, runtimeElement));
        }
    }
}