using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Common.Mocks;

namespace UniverseBuilder.Tests
{
    [TestClass]
    public class TestUniverseBuilder
    {
        [TestMethod]
        public async Task Test_Build_Universe()
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
            var universeDefinition = await universeBuilder.BuildUniverseAsync("eventStreamFilePathpath", universeTemplate);

            var hasUniverseServiceEndpoints = universeDefinition?.ServiceEndpoints.Count > 0 ? true : false;

            Assert.IsTrue(hasUniverseServiceEndpoints);
        }
    }
}
