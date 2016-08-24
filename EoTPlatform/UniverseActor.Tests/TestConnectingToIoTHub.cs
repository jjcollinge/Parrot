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

        [TestMethod]
        public async Task TestSuccesfulIoTHubConnectionAndDeviceRemoval()
        {
            var actor = new UniverseActor();
            var method1 = typeof(ActorBase).GetMethod("OnActivateAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            await (Task)method1.Invoke(actor, null);
            var connected = await actor.IsConnectedAsync();
            Assert.IsTrue(connected);

            var method2 = typeof(ActorBase).GetMethod("OnDeactivateAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            await (Task)method2.Invoke(actor, null);
            connected = await actor.IsConnectedAsync();
            Assert.IsFalse(connected);
        }

        //[TestMethod]
        //public async Task TestActorReminders()
        //{
        //    var actor = new UniverseActor();
        //    var method1 = typeof(ActorBase).GetMethod("OnActivateAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        //    await (Task)method1.Invoke(actor, null);
        //    await actor.ReceiveReminderAsync("test", null, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
        //    Assert.IsTrue(true);
        //}

        [TestMethod]
        public async Task TestIoTHubMessageSend()
        {
            var actor = new UniverseActor();
            var method1 = typeof(ActorBase).GetMethod("OnActivateAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            await (Task)method1.Invoke(actor, null);
            await actor.SendMessageAsync("Hello World");

            var method2 = typeof(ActorBase).GetMethod("OnDeactivateAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            await (Task)method2.Invoke(actor, null);

            Assert.IsTrue(true);
        }
    }
}
