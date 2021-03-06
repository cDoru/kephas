﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDiscardChangesCommand.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Declares the IDiscardChangesCommand interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Data.Commands
{
    using Kephas.Services;

    /// <summary>
    /// Contract for discard changes commands.
    /// </summary>
    [AppServiceContract(AllowMultiple = true, MetadataAttributes = new[] { typeof(DataContextTypeAttribute) })]
    public interface IDiscardChangesCommand : IDataCommand<IDataOperationContext, IDataCommandResult>
    {
    }
}