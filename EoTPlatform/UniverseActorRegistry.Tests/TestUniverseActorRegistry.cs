using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using System.Collections.Generic;
using Common;
using System.Fabric;
using System;
using Common.Mocks;

namespace UniverseActorRegistry.Tests
{
    [TestClass]
    public class TestUniverseActorRegistry
    {
        private IUniverseActorRegistry registry;
        private string actorIdAsString0 = "0";
        private string actorIdAsString1 = "1";
        private string actorIdAsString2 = "2";

        public TestUniverseActorRegistry()
        {
            var context = this.CreateServiceContext();
            registry = new UniverseActorRegistry(context, new MockReliableStateManager());
        }

        [TestMethod]
        public async Task Test_Register_Universe_Actor()
        {
            var hasRegisteredActor = await registry.RegisterUniverseActorAsync(actorIdAsString0, ActorId.CreateRandom());
            Assert.IsTrue(hasRegisteredActor);
        }

        [TestMethod]
        public async Task Test_Register_Universe_Actor_With_Duplicate_Id()
        {
            var hasRegisteredActor = await registry.RegisterUniverseActorAsync(actorIdAsString0, ActorId.CreateRandom());
            Assert.IsFalse(hasRegisteredActor);
        }

        [TestMethod]
        public async Task Test_Register_Multiple_Universe_Actors()
        {
            var hasRegisteredActors = await registry.RegisterUniverseActorAsync(actorIdAsString1, ActorId.CreateRandom());
            Assert.IsTrue(hasRegisteredActors);
            hasRegisteredActors = await registry.RegisterUniverseActorAsync(actorIdAsString2, ActorId.CreateRandom());
            Assert.IsTrue(hasRegisteredActors);
        }

        [TestMethod]
        public async Task Test_Get_All_Registered_Universe_Actors()
        {
            var users = await registry.GetAllRegisteredUniverseActorsAsync();
            var hasGottenUsers = users.Count > 0 ? true : false;
            Assert.IsTrue(hasGottenUsers);
        }

        [TestMethod]
        public async Task Test_Get_Registered_Universe_Actor()
        {
            var user = await registry.GetRegisteredUniverseActorAsync(actorIdAsString0);
            var hasGottenUser = user.Key != null || user.Value != null ? true : false;
            Assert.IsTrue(hasGottenUser);
        }

        [TestMethod]
        public async Task Test_Get_Registered_Universe_Actor_With_Non_Existent_Id()
        {
            var user = await registry.GetRegisteredUniverseActorAsync("Invalid_Id");
            var hasGottenUser = user.Key != null || user.Value != null ? true : false;
            Assert.IsFalse(hasGottenUser);
        }

        [TestMethod]
        public async Task Test_Deregister_Universe_Actor()
        {
            var hasDeregisteredUsers = await registry.DeregisterUniverseActorAsync(actorIdAsString0);
            Assert.IsTrue(hasDeregisteredUsers);
        }

        [TestMethod]
        public async Task Test_Deregister_Universe_Actor_With_Non_Existent_Id()
        {
            var hasDeregisteredUsers = await registry.DeregisterUniverseActorAsync(actorIdAsString0);
            Assert.IsFalse(hasDeregisteredUsers);
        }

        [TestMethod]
        public async Task Test_Deregister_All_Universe_Actors()
        {
            await registry.ClearAllAsync<ActorId>();
            var users = await registry.GetAllRegisteredUniverseActorsAsync();
            var hasDeregisteredAllUsers = users.Count == 0 ? true : false;
            Assert.IsTrue(hasDeregisteredAllUsers);
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
