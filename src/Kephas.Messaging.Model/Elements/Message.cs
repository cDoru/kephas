﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the message class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Messaging.Model.Elements
{
    using Kephas.Model.Construction;
    using Kephas.Model.Elements;

    /// <summary>
    /// Classifier for DTOs used in messaging.
    /// </summary>
    public class Message : ClassifierBase<IMessage>, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="constructionContext">Context for the construction.</param>
        /// <param name="name">The name.</param>
        public Message(IModelConstructionContext constructionContext, string name)
            : base(constructionContext, name)
        {
        }
    }
}