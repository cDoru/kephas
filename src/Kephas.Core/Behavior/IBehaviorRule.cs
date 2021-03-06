﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBehaviorRule.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Contracts for defining rules for behavior values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Behavior
{
    /// <summary>
    /// Non-generic contract for defining a behavior rule.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IBehaviorRule<in TContext> : IBehaviorRuleFlowControl
    {
        /// <summary>
        /// Gets a value indicating whether the rule may be applied or not.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A value indicating whether the rule may be applied or not.
        /// </returns>
        bool CanApply(TContext context);

        /// <summary>
        /// Gets the behavior value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The behavior value.
        /// </returns>
        IBehaviorValue GetValue(TContext context);
    }

    /// <summary>
    /// Contract for defining a behavior rule.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    /// <typeparam name="TValue">  The type of the behavior value.</typeparam>
    public interface IBehaviorRule<in TContext, out TValue> : IBehaviorRule<TContext>
    {
        /// <summary>
        /// Gets the behavior value asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A promise of the behavior value.
        /// </returns>
        new IBehaviorValue<TValue> GetValue(TContext context);
    }
}