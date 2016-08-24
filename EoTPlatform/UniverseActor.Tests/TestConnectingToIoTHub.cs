using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using System.Reflection;

namespace UniverseActor.Tests
{
    [TestClass]
    public class TestConnectingToIoTHub
    {
        /**
         * TODO: Requires further abstractions to mock components
         **/

        [TestMethod]
        public async Task TestSuccesfulIoTHubConnection()
        {
            var actor = new UniverseActor();
            var method = typeof(ActorBase).GetMethod("OnActivateAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            await (Task)method.Invoke(actor, null);
            var connected = await actor.IsConnectedAsync();
            Assert.IsTrue(connected);
        }
    }
}
