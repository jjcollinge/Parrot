using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Mocks;
using Microsoft.ServiceFabric.Data;
using System.Fabric;

namespace UniverseRegistry.Tests
{
    [TestClass]
    public class TestUniverseRegistry
    {
        private UniverseRegistry registry;
        private UniverseDefinition definition;

        public TestUniverseRegistry()
        {
            var context = this.CreateServiceContext();
            this.registry = new UniverseRegistry(context, new ReliableStateManager(context));
            this.definition = new UniverseDefinition();
            this.definition.Id = "mock";
            this.definition.Status = UniverseStatus.Running;
            this.definition.ServiceEndpoints.Add("mock", new List<string> { "localhost:8080" });
        }

        [TestMethod]
        public async Task Test_Register_Universe()
        {
            // Will throw due to null context
            var hasRegisteredUniverse = await registry.RegisterUniverseAsync(definition.Id, definition);
            Assert.IsTrue(hasRegisteredUniverse);
        }

        [TestMethod]
        public async Task Test_Deregister_Universe()
        {
            // Will throw due to null context
            var hasDeregisteredUniverse = await registry.DeregisterUniverseAsync(definition.Id);
            Assert.IsTrue(hasDeregisteredUniverse);
        }

        [Ignore]
        private StatefulServiceContext CreateServiceContext()
        {
            return new StatefulServiceContext(
                new NodeContext(String.Empty, new NodeId(0, 0), 0, String.Empty, String.Empty),
                new MockCodePackageActivationContext(),
                String.Empty,
                new Uri("fabric:/Mock"),
                null,
                Guid.NewGuid(),
                0);
        }
    }
}
