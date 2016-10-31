﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullAppEnvironment.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Implements the default application environment class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Application
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The <c>null</c> application environment.
    /// </summary>
    public class NullAppEnvironment : AppEnvironmentBase
    {
        /// <summary>
        /// Gets the loaded assemblies.
        /// </summary>
        /// <returns>
        /// The loaded assemblies.
        /// </returns>
        protected override IList<Assembly> GetLoadedAssemblies()
        {
            return new List<Assembly> { this.GetType().GetTypeInfo().Assembly };
        }

        /// <summary>
        /// Gets the referenced assemblies.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// An array of assembly name.
        /// </returns>
        protected override AssemblyName[] GetReferencedAssemblies(Assembly assembly)
        {
            return new AssemblyName[0];
        }
    }
}