﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrExpressionConverter.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the or expression converter class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Client.Queries.Conversion.ExpressionConverters
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Expression converter for the OR operator.
    /// </summary>
    [Operator("$or")]
    public class OrExpressionConverter : IExpressionConverter
    {
        /// <summary>
        /// Converts the provided expression to a LINQ expression.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The converted expression.
        /// </returns>
        public Expression ConvertExpression(IList<Expression> args)
        {
            return args.Count == 0 ? null : this.JoinExpressions(args, 0);
        }

        /// <summary>
        /// Joins the expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="index">Zero-based index of the expression to be joined.</param>
        /// <returns>
        /// An Expression.
        /// </returns>
        private Expression JoinExpressions(IList<Expression> expressions, int index)
        {
            if (index == expressions.Count - 1)
            {
                return expressions[expressions.Count - 1];
            }

            return Expression.OrElse(expressions[index], this.JoinExpressions(expressions, index + 1));
        }
    }
}