using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Mocks;
using Common.Models;
using System.Threading.Tasks;

namespace UniverseFactory.Tests
{
    [TestClass]
    public class TestCreatingUniverseFromSpecification
    {
        [TestMethod]
        public async Task TestSuccessfulUniverseCreation()
        {
            var specification = new UniverseSpecification();
            specification.Id = "universe123";
            specification.UniverseTemplateFilePath = "tess";

            var universeFactory = new UniverseFactory(null, new MockServiceProxyFactory());
            var universeDescriptor = await universeFactory.CreateUniverseAsync(specification);
            Assert.IsTrue(universeDescriptor.ServiceEndpoints != null);
        }
    }
}
