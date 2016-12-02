﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFindCommand.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Declares the IFindCommand interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Commands
{
    using Kephas.Services;

    /// <summary>
    /// Contract for find commands.
    /// </summary>
    /// <typeparam name="TDataContext">Type of the data context.</typeparam>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    [AppServiceContract(AsOpenGeneric = true)]
    public interface IFindCommand<in TDataContext, TEntity> : IDataCommand<IFindContext, IFindResult<TEntity>>
        where TDataContext : IDataContext
        where TEntity : class
    {
    }
}