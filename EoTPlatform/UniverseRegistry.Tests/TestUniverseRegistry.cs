using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Mocks;

namespace UniverseRegistry.Tests
{
    [TestClass]
    public class TestUniverseRegistry
    {
        [TestMethod]
        public async Task TestRegisterUniverseAsync()
        {
            var registry = new UniverseRegistry(null, new MockReliableStateManager());

            UniverseDefinition definition = new UniverseDefinition();
            definition.Id = "mock";
            definition.Status = Status.Running;
            definition.ServiceEndpoints.Add("mock", new List<string> { "localhost:8080" });

            await registry.RegisterUniverseAsync(definition);

            var universe = await registry.GetUniverseAsync(definition.Id);

            Assert.IsTrue(definition.Status == universe.Status);
            Assert.IsTrue(definition.ServiceEndpoints == universe.ServiceEndpoints);
        }

        [TestMethod]
        public async Task TestDeregisterUniverseAsync()
        {
            var registry = new UniverseRegistry(null, new MockReliableStateManager());

            UniverseDefinition definition = new UniverseDefinition();
            definition.Id = "mock";
            definition.Status = Status.Running;
            definition.ServiceEndpoints.Add("mock", new List<string> { "localhost:8080" });

            await registry.RegisterUniverseAsync(definition);
            await registry.DeregisterUniverseAsync(definition.Id);

            var universes = await registry.GetUniversesAsync();

            Assert.IsTrue(universes.Count == 0);
        }
    }
}
