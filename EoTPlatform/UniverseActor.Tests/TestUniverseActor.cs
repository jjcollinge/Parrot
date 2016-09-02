using Common.Mocks;
using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniverseActor.Tests
{
    [TestClass]
    public class TestUniverseActor
    {
        [TestMethod]
        public async Task Test_Setup()
        {
            var actor = new UniverseActor(new MockCloudConnector());

            var template = new ActorTemplate("0");
            template.Metadata.Add("route", "6");
            template.Transformations.Add("x", 2.0);
            template.Commands.Add("rotateX");

            await actor.SetupAsync(template);

            Assert.Fail();
        }

        [TestMethod]
        public async Task Test_Process_Event()
        {
            var actor = new UniverseActor(new MockCloudConnector());

            var template = new ActorTemplate("0");
            template.Metadata.Add("route", "6");
            template.Transformations.Add("x", 2.0);
            template.Commands.Add("rotateX");

            var evt = new UniverseEvent();
            evt.ActorId = "0";
            evt.OriginalTimeStamp = DateTime.Now;
            evt.Payload = new KeyValuePair<string, double>("x", 2.0);
            await actor.ProcessEventAsync(evt);
        }
    }
}
