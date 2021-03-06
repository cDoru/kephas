﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBehaviorValue.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Generic contract for behavior values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Behavior
{
    using Kephas.Dynamic;

    /// <summary>
    /// Generic contract for behavior values.
    /// </summary>
    public interface IBehaviorValue : IExpando
    {
        /// <summary>
        /// Gets the behavior value.
        /// </summary>
        /// <value>
        /// The behavior value.
        /// </value>
        object Value { get; }
    }

    /// <summary>
    /// Generic contract for behavior values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IBehaviorValue<out TValue> : IBehaviorValue
    {
        /// <summary>
        /// Gets the behavior value.
        /// </summary>
        /// <value>
        /// The behavior value.
        /// </value>
        new TValue Value { get; }
    }
}