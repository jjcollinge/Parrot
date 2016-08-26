using Common.Mocks;
using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniverseActor.Tests
{
    [TestClass]
    public class TestUniverseActor
    {
        [TestMethod]
        public async Task TestSetupAsyncSuccess()
        {
            var actor = new UniverseActor(new MockCloudConnector());

            var template = new ActorTemplate("0");
            template.Metadata.Add("route", "6");
            template.Properties.Add("x", new ActorTemplateProperty() { Value = "1", Type = "int", Unit = "metres" });
            template.Commands.Add("rotateX");

            await actor.SetupAsync(template);

            Assert.Fail();
        }
    }
}
