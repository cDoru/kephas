﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MixinAnnotation.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the mixin annotation class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Model.Elements.Annotations
{
    using Kephas.Model.Construction;

    /// <summary>
    /// Annotation for mix-in classifiers.
    /// </summary>
    public class MixinAnnotation : Annotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MixinAnnotation"/> class.
        /// </summary>
        /// <param name="constructionContext">Context for the construction.</param>
        /// <param name="name">The model element name.</param>
        public MixinAnnotation(IModelConstructionContext constructionContext, string name)
            : base(constructionContext, name)
        {
        }
    }
}