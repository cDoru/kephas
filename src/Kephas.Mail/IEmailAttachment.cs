﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEmailAttachment.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Declares the IEmailAttachment interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Mail
{
    using System.IO;

    using Kephas.Dynamic;

    /// <summary>
    /// An email attachment.
    /// </summary>
    public interface IEmailAttachment : IExpando
    {
        /// <summary>
        /// Gets or sets the attachment name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the attachment content.
        /// </summary>
        /// <value>
        /// The attachment content.
        /// </value>
        Stream Content { get; set; }
    }
}