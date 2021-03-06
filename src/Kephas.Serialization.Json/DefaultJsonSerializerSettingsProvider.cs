﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultJsonSerializerSettingsProvider.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   A default JSON serializer settings provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Dynamic;
    using System.Linq;

    using Kephas.Collections;
    using Kephas.Diagnostics.Contracts;
    using Kephas.Logging;
    using Kephas.Reflection;
    using Kephas.Runtime;
    using Kephas.Serialization.Json.Converters;
    using Kephas.Serialization.Json.Resources;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// A default JSON serializer settings provider.
    /// </summary>
    public class DefaultJsonSerializerSettingsProvider : IJsonSerializerSettingsProvider
    {
        /// <summary>
        /// The static instance.
        /// </summary>
        private static DefaultJsonSerializerSettingsProvider instance;

        /// <summary>
        /// The JSON converters.
        /// </summary>
        private readonly ICollection<JsonConverter> jsonConverters;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializerSettingsProvider"/> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="jsonConverters">The JSON converters (optional).</param>
        public DefaultJsonSerializerSettingsProvider(ITypeResolver typeResolver, ICollection<IJsonConverter> jsonConverters = null)
        {
            Requires.NotNull(typeResolver, nameof(typeResolver));

            this.jsonConverters = jsonConverters?.OfType<JsonConverter>().ToList()
                                    ?? new List<JsonConverter>
                                                 {
                                                     new DateTimeJsonConverter(),
                                                     new TimeSpanJsonConverter(),
                                                     new StringEnumJsonConverter(),
                                                 };
            this.TypeResolver = typeResolver;
        }

        /// <summary>
        /// Gets the default instance of this settings provider.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DefaultJsonSerializerSettingsProvider Instance => instance ?? (instance = CreateDefaultInstance());

        /// <summary>
        /// Creates a default instance.
        /// </summary>
        /// <returns>
        /// The new instance.
        /// </returns>
        private static DefaultJsonSerializerSettingsProvider CreateDefaultInstance()
        {
            var ambientServices = AmbientServices.Instance;
            var instance = new DefaultJsonSerializerSettingsProvider(ambientServices.GetService<ITypeResolver>())
            {
                Logger = ambientServices.GetLogger<DefaultJsonSerializerSettingsProvider>()
            };

            return instance;
        }

        /// <summary>
        /// Gets the type resolver.
        /// </summary>
        /// <value>
        /// The type resolver.
        /// </value>
        public ITypeResolver TypeResolver { get; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger<DefaultJsonSerializerSettingsProvider> Logger { get; set; }

        /// <summary>
        /// Gets the JSON serializer settings.
        /// </summary>
        /// <returns>
        /// The JSON serializer settings.
        /// </returns>
        public JsonSerializerSettings GetJsonSerializerSettings()
        {
            return this.GetJsonSerializerSettings(camelCase: true, thrownOnMissingMembers: true, converters: this.jsonConverters);
        }

        /// <summary>
        /// Gets the json serializer settings.
        /// </summary>
        /// <param name="camelCase">If set to <c>true</c> the ccamel case will be used for properties.</param>
        /// <param name="thrownOnMissingMembers">If set to <c>true</c> [thrown on missing members].</param>
        /// <param name="converters">The json converters.</param>
        /// <returns>
        /// The json serializer settings.
        /// </returns>
        protected virtual JsonSerializerSettings GetJsonSerializerSettings(
            bool camelCase,
            bool thrownOnMissingMembers = true,
            IEnumerable<JsonConverter> converters = null)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ////PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ////ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                MissingMemberHandling = thrownOnMissingMembers
                                            ? MissingMemberHandling.Error
                                            : MissingMemberHandling.Ignore,
                Error = this.HandleJsonSerializationError,
                ContractResolver = this.GetContractResolver(camelCase),
                SerializationBinder = this.GetSerializationBinder(),
            };

            serializerSettings.Converters.AddRange(converters ?? this.jsonConverters);

            return serializerSettings;
        }

        /// <summary>
        /// Gets the serialization binder.
        /// </summary>
        /// <returns>
        /// The serialization binder.
        /// </returns>
        protected virtual ISerializationBinder GetSerializationBinder()
        {
            return new TypeResolverSerializationBinder(this.TypeResolver);
        }

        /// <summary>
        /// Gets the contract resolver.
        /// </summary>
        /// <param name="camelCase">If set to <c>true</c> the camel case will be used for properties.</param>
        /// <returns>
        /// The contract resolver.
        /// </returns>
        protected virtual IContractResolver GetContractResolver(bool camelCase)
        {
            Contract.Ensures(Contract.Result<IContractResolver>() != null);

            return camelCase ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver();
        }

        /// <summary>
        /// Error handler for json deserialization errors.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void HandleJsonSerializationError(object sender, ErrorEventArgs args)
        {
            this.Logger.Error(
                args.ErrorContext.Error,
                Strings.DefaultJsonSerializerSettingsProvider_ErrorOnSerializingObjectMessage,
                args.CurrentObject?.GetType());
        }
    }
}