﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpandoBase.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Class that provides extensible properties and methods. This
//   dynamic object stores 'extra' properties in a dictionary or
//   checks the actual properties of the instance.
//   This means you can subclass this expando and retrieve either
//   native properties or properties from values in the dictionary.
//   This type allows you three ways to access its properties:
//   Directly: any explicitly declared properties are accessible
//   Dynamic: dynamic cast allows access to dictionary and native properties/methods
//   Dictionary: Any of the extended properties are accessible via IDictionary interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    using Kephas.Diagnostics.Contracts;
    using Kephas.Reflection;
    using Kephas.Resources;
    using Kephas.Runtime;

    /// <summary>
    /// <para>
    /// Class that provides extensible properties and methods. This
    /// dynamic object stores 'extra' properties in a dictionary or
    /// checks the actual properties of the instance.
    /// This means you can subclass this expando and retrieve either
    /// native properties or properties from values in the dictionary.
    /// </para>
    /// <para>
    /// This type allows you three ways to access its properties:
    /// <list type="bullet">
    /// <item>
    /// <term>Directly</term>
    /// <description>any explicitly declared properties are accessible</description>
    /// </item>
    /// <item>
    /// <term>Dynamic</term>
    /// <description>dynamic cast allows access to dictionary and native properties/methods</description>
    /// </item>
    /// <item>
    /// <term>Dictionary</term>
    /// <description>Any of the extended properties are accessible via IDictionary interface</description>
    /// </item>
    /// </list> 
    /// </para>
    /// </summary>
    public abstract class ExpandoBase : DynamicObject, IExpando
    {
        /// <summary>
        /// Value of object passed in.
        /// </summary>
        private object innerObject;

        /// <summary>
        /// The inner dictionary for dynamic values.
        /// </summary>
        private IDictionary<string, object> innerDictionary;

        /// <summary>
        /// Cached dynamic type of the inner object.
        /// </summary>
        /// <remarks>
        /// Do not use directly this field, instead use the <see cref="GetInnerObjectTypeInfo"/> method
        /// which knows how to late-initialize it.
        /// </remarks>
        private IRuntimeTypeInfo innerObjectTypeInfo;

        /// <summary>
        /// Cached dynamic type of this instance.
        /// </summary>
        /// <remarks>
        /// Do not use directly this field, instead use the <see cref="GetThisTypeInfo"/> method
        /// which knows how to late-initialize it.
        /// </remarks>
        private IRuntimeTypeInfo thisTypeInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandoBase"/> class.
        /// This constructor just works off the internal dictionary.
        /// </summary>
        /// <param name="innerDictionary">
        /// The inner dictionary for holding dynamic values (optional).
        /// If not provided, a new dictionary will be created.
        /// </param>
        protected ExpandoBase(IDictionary<string, object> innerDictionary = null)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            this.InitializeExpando(null, innerDictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandoBase"/> class. Allows passing in an
        /// existing instance variable to 'extend'.
        /// </summary>
        /// <param name="innerObject">The instance to be extended.</param>
        /// <param name="innerDictionary">
        /// The inner dictionary for holding dynamic values (optional).
        /// If not provided, a new dictionary will be created.
        /// </param>
        protected ExpandoBase(object innerObject, IDictionary<string, object> innerDictionary = null)
        {
            Requires.NotNull(innerObject, nameof(innerObject));

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            this.InitializeExpando(innerObject, innerDictionary);
        }

        /// <summary>
        /// Convenience method that provides a string Indexer to the Properties collection AND the
        /// strongly typed properties of the object by name. // dynamic exp["Address"] = "112 nowhere
        /// lane";
        /// // strong var name = exp["StronglyTypedProperty"] as string;.
        /// </summary>
        /// <remarks>
        /// The getter checks the Properties dictionary first then looks in PropertyInfo for properties.
        /// The setter checks the instance properties before checking the Properties dictionary.
        /// </remarks>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="object" />.</returns>
        public object this[string key]
        {
            get
            {
                object value;
                this.TryGetValue(key, out value);
                return value;
            }

            set
            {
                this.TrySetValue(key, value);
            }
        }

        /// <summary>
        /// Try to retrieve a member by name first from instance properties
        /// followed by the collection entries.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        /// Property setter implementation tries to retrieve value from instance
        /// first then into this object.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, the <paramref name="value" /> is "Test".</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return this.TrySetValue(binder.Name, value);
        }

        /// <summary>
        /// Dynamic invocation method. Currently allows only for Reflection based
        /// operation (no ability to add methods dynamically).
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="args" />[0] is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (this.innerObject != null
                && this.GetInnerObjectTypeInfo().TryInvoke(this.innerObject, binder.Name, args, out result))
            {
                return true;
            }

            if (this.GetThisTypeInfo().TryInvoke(this, binder.Name, args, out result))
            {
                return true;
            }

            object method;
            if (this.innerDictionary.TryGetValue(binder.Name, out method))
            {
                var delegateProperty = method as Delegate;
                if (delegateProperty == null)
                {
                    throw new MemberAccessException(string.Format(Strings.ExpandoBase_CannotInvokeNonDelegate_Exception, binder.Name, delegateProperty?.GetType()));
                }

                result = delegateProperty.DynamicInvoke(args);
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Converts the expando to a dictionary having as keys the property names and as values the respective properties' values.
        /// </summary>
        /// <returns>
        /// A dictionary of property values with their associated names.
        /// </returns>
        public Dictionary<string, object> ToDictionary()
        {
            // add the properties in their overwrite order:
            // first, the values in the dictionary
            var dictionary = new Dictionary<string, object>(this.innerDictionary);

            // second, the values in this expando's properties
            foreach (var prop in this.GetThisTypeInfo().Properties)
            {
                dictionary.Add(prop.Key, prop.Value.GetValue(this.innerObject));
            }

            // last, the values in the inner object
            if (this.innerObject != null)
            {
                foreach (var prop in this.GetInnerObjectTypeInfo().Properties)
                {
                    dictionary.Add(prop.Key, prop.Value.GetValue(this.innerObject));
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the <see cref="IRuntimeTypeInfo"/> of the inner object.
        /// </summary>
        /// <returns>
        /// The <see cref="IRuntimeTypeInfo"/> of the inner object.
        /// </returns>
        protected virtual IRuntimeTypeInfo GetInnerObjectTypeInfo()
        {
            return this.innerObject == null
                       ? null
                       : this.innerObjectTypeInfo ?? (this.innerObjectTypeInfo = this.innerObject.GetType().AsRuntimeTypeInfo());
        }

        /// <summary>
        /// Gets the <see cref="IRuntimeTypeInfo"/> of this expando object.
        /// </summary>
        /// <returns>
        /// The <see cref="IRuntimeTypeInfo"/> of this expando object.
        /// </returns>
        protected virtual IRuntimeTypeInfo GetThisTypeInfo()
        {
            return this.thisTypeInfo ?? (this.thisTypeInfo = this.GetType().AsRuntimeTypeInfo());
        }

        /// <summary>
        /// Attempts to get the dictionary value from the given data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to get from the dictionary.</param>
        /// <returns>
        /// <c>true</c> if a value is found, <c>false</c> otherwise.
        /// </returns>
        protected virtual bool TryGetValue(string key, out object value)
        {
            IRuntimePropertyInfo propInfo;

            // First check for public properties via reflection
            if (this.innerObject != null)
            {
                if (this.GetInnerObjectTypeInfo().Properties.TryGetValue(key, out propInfo))
                {
                    value = propInfo.GetValue(this.innerObject);
                    return true;
                }
            }

            // then, check the properties in this object
            if (this.GetThisTypeInfo().Properties.TryGetValue(key, out propInfo))
            {
                value = propInfo.GetValue(this);
                return true;
            }

            // last, check the dictionary for member
            if (this.innerDictionary.TryGetValue(key, out value))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to set the gived data in the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to set into the dictionary.</param>
        /// <returns>
        /// <c>true</c> if the value could be set, <c>false</c> otherwise.
        /// </returns>
        protected virtual bool TrySetValue(string key, object value)
        {
            IRuntimePropertyInfo propInfo;

            // First check for public properties via reflection
            if (this.innerObject != null)
            {
                if (this.GetInnerObjectTypeInfo().Properties.TryGetValue(key, out propInfo))
                {
                    propInfo.SetValue(this.innerObject, value);
                    return true;
                }
            }

            // then, check the properties in this object
            if (this.GetThisTypeInfo().Properties.TryGetValue(key, out propInfo))
            {
                propInfo.SetValue(this, value);
                return true;
            }

            // last, check the dictionary for member
            this.innerDictionary[key] = value;
            return true;
        }

        /// <summary>
        /// Initializes the expando with the provided instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="dictionary">The inner dictionary.</param>
        private void InitializeExpando(object instance, IDictionary<string, object> dictionary)
        {
            this.innerObject = instance;
            this.innerDictionary = dictionary ?? new Dictionary<string, object>();
        }
    }
}
