using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Fabric;

namespace UniverseBuilder.Tests
{
    [TestClass]
    public class TestUniverseBuilderUsingUniverseTemplate
    {
        [TestMethod]
        public async Task TestBuildingUniverseFromUniverseTemplate()
        {
            var universeTemplate = new UniverseTemplate();
            universeTemplate.Id = "0";
            universeTemplate.ActorTemplates = new List<ActorTemplate>
            {
                new ActorTemplate { Id="0", Type="train", Publishers= new List<string> { "1" } },
                new ActorTemplate { Id="1", Type="loop", Publishers= new List<string> { "0" } }
            };

            var platform = new MockPlatformWrapper();
            var factory = new MockServiceProxyFactory();
            var universeBuilder = new UniverseBuilder(null, platform, factory);
            var universeDescriptor = await universeBuilder.BuildUniverseAsync(universeTemplate);
            Assert.IsNotNull(universeDescriptor);
            Assert.IsNotNull(universeDescriptor.ServiceEndpoints);
            Assert.IsTrue(universeDescriptor.ServiceEndpoints.Count == 1);
        }
    }
}
