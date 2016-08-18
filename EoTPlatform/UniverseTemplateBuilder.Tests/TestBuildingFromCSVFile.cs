using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;
using Common.Services.Models;
using Newtonsoft.Json;

namespace UniverseTemplateBuilder.Tests
{
    [TestClass]
    public class TestBuildingFromCSVFile
    {
        [TestMethod]
        public async Task TestBuildUniverseTemplateFromCSV()
        {
            var universeTemplateBuilder = new UniverseTemplateBuilder(null);
            var inputFilePath = $"{GetProjectFilePath()}\\csv\\testData.csv";
            var outputFilePath = $"{GetProjectFilePath()}\\templates\\outputTemplate.json";
            var universeTemplateJson = await universeTemplateBuilder.BuildUniverseTemplateFromFileAsync(inputFilePath);
            var universeTemplate = JsonConvert.DeserializeObject<UniverseTemplate>(universeTemplateJson);

            Assert.IsNotNull(universeTemplate);
            Assert.IsNotNull(universeTemplate.Id);
            Assert.IsNotNull(universeTemplate.ActorTemplates);
            Assert.IsTrue(universeTemplate.ActorTemplates.Count == 11);
        }

        [Ignore]
        public string GetProjectFilePath()
        {
            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            return projectFolderPath;
        }
    }
}
