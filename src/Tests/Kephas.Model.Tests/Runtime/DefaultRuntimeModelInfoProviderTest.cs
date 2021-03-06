﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRuntimeModelInfoProviderTest.cs" company="Quartz Software SRL">
//   Copyright (c) Quartz Software SRL. All rights reserved.
// </copyright>
// <summary>
//   Tests for <see cref="DefaultRuntimeModelInfoProvider" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.Model.Tests.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Model.Construction;
    using Kephas.Model.Runtime;
    using Kephas.Model.Runtime.Construction;

    using NSubstitute;

    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="DefaultRuntimeModelInfoProvider"/>.
    /// </summary>
    [TestFixture]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class DefaultRuntimeModelInfoProviderTest
    {
        [Test]
        public async Task GetElementInfosAsync()
        {
            var registrar = Substitute.For<IRuntimeModelRegistry>();
            registrar.GetRuntimeElementsAsync(CancellationToken.None).Returns(Task.FromResult((IEnumerable<object>)new object[] { typeof(string).GetRuntimeTypeInfo() }));

            var stringInfoMock = Substitute.For<INamedElement>();

            var factory = Substitute.For<IRuntimeModelElementFactory>();
            factory.TryCreateModelElement(Arg.Any<IModelConstructionContext>(), Arg.Is(typeof(string).GetRuntimeTypeInfo())).Returns(stringInfoMock);

            var provider = new DefaultRuntimeModelInfoProvider(factory, new[] { registrar });

            var elementInfos = (await provider.GetElementInfosAsync(Substitute.For<IModelConstructionContext>())).ToList();

            Assert.AreEqual(1, elementInfos.Count);
            Assert.AreSame(stringInfoMock, elementInfos[0]);
        }
    }
}