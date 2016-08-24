using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Common.Mocks;

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
            universeTemplate.Version = "0.1";
            universeTemplate.ActorTemplates = new List<ActorTemplate>
            {
                new ActorTemplate("0"),
                new ActorTemplate("1")
            };

            var platform = new MockPlatformAbstraction();
            var factory = new MockServiceProxyFactory();
            var universeBuilder = new UniverseBuilder(null, platform, factory);
            var universeDescriptor = await universeBuilder.BuildUniverseAsync(universeTemplate);
            Assert.IsNotNull(universeDescriptor);
            Assert.IsNotNull(universeDescriptor.ServiceEndpoints);
            Assert.IsTrue(universeDescriptor.ServiceEndpoints.Count == 1);
        }
    }
}
