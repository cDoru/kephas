﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExport.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Contract for a handle allowing the graph of parts associated with an exported instance
//   to be released.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Composition
{
    using System;

    /// <summary>
    /// Non-generic contract for a handle allowing the graph of parts associated with an exported instance
    /// to be released.
    /// </summary>
    public interface IExport : IDisposable
    {
        /// <summary>
        /// Gets the exported value.
        /// </summary>
        /// <value>
        /// The exported value.
        /// </value>
        object Value { get; }
    }

    /// <summary>
    /// Contract for a handle allowing the graph of parts associated with an exported instance
    /// to be released.
    /// </summary>
    /// <typeparam name="T">The contract type of the created parts.</typeparam>    
    public interface IExport<out T> : IExport
    {
        /// <summary>
        /// Gets the exported value.
        /// </summary>
        /// <value>
        /// The exported value.
        /// </value>
        new T Value { get; }
    }

    /// <summary>
    /// Contract for a handle allowing the graph of parts associated with an exported instance to be
    /// released.
    /// </summary>
    /// <typeparam name="T">The contract type of the created parts.</typeparam>
    /// <typeparam name="TMetadata">Type of the metadata.</typeparam>
    public interface IExport<out T, out TMetadata> : IExport<T>
    {
        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        TMetadata Metadata { get; }
    }
}