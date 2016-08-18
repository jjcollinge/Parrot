using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Common.Models;
using System.IO;

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
            var universeTemplateJson = await universeTemplateBuilder.BuildUniverseTemplateFromFileAsync(inputFilePath);
            var universeTemplate = JsonConvert.DeserializeObject<UniverseTemplate>(universeTemplateJson);

            Assert.IsNotNull(universeTemplate);
            Assert.IsNotNull(universeTemplate.Id);
            Assert.IsNotNull(universeTemplate.ActorTemplates);
            Assert.IsTrue(universeTemplate.ActorTemplates.Count == 11);
        }

        [TestMethod]
        public async Task TestBuildUniverseTemplateFromCSVAndWritingToJson()
        {
            var universeTemplateBuilder = new UniverseTemplateBuilder(null);
            var inputFilePath = $"{GetProjectFilePath()}\\csv\\testData.csv";
            var outputFilePath = $"{GetProjectFilePath()}\\templates\\outputTemplate.json";
            await universeTemplateBuilder.BuildUniverseTemplateFromFileAsync(inputFilePath, outputFilePath);

            Assert.IsTrue(File.Exists(outputFilePath));

            UniverseTemplate template = null;
            using (StreamReader r = new StreamReader(outputFilePath))
            {
                string json = r.ReadToEnd();
                template = JsonConvert.DeserializeObject<UniverseTemplate>(json);
            }

            Assert.IsTrue(template.ActorTemplates.Count == 11);
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
