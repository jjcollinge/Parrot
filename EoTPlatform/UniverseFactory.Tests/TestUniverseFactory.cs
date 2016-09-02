using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Mocks;
using Common.Models;
using System.Threading.Tasks;

namespace UniverseFactory.Tests
{
    [TestClass]
    public class TestUniverseFactory
    {
        private static string testFileName = "testTemplate.json";
        private static string testEventStream = "testEventStream.csv";

        [TestMethod]
        public async Task Test_Create_Universe()
        {
            var specification = new UniverseSpecification();
            specification.Id = "universe123";
            specification.UniverseTemplateFilePath = testFileName;
            specification.UniverseEventStreamFilePath = testEventStream;

            var universeFactory = new UniverseFactory(null, new MockServiceProxyFactory());
            var universeDefinition = await universeFactory.CreateUniverseAsync(specification);

            var hasCreatedUniverseDefinition = universeDefinition != null ? true : false;

            Assert.IsTrue(hasCreatedUniverseDefinition);
        }

        [Ignore]
        public string GetTemplatePath(string templateFileName)
        {
            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string templatePath = $"{projectFolderPath}\\templates\\{templateFileName}";
            return templatePath;
        }
    }
}
