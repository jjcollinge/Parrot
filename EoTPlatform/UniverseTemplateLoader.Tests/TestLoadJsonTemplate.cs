using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Fabric;
using System.Threading.Tasks;
using System.IO;

namespace UniverseTemplateLoader.Tests
{
    [TestClass]
    public class TestLoadJsonTemplate
    {
        private static string testTemplateId = "universe123124";
        private static string testFileName = "testTemplate.json";

        [TestMethod]
        public async Task LoadJsonTemplateFromFileAsync()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(testFileName));
            Assert.IsTrue(template.Id == testTemplateId);
            Assert.IsNotNull(template.ActorTemplates);
        }

        [TestMethod]
        public async Task LoadJsonTemplateFromFileAndCheckTemplateName()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(testFileName));
            Assert.IsTrue(template.Id == testTemplateId);
        }

        [TestMethod]
        public async Task LoadJsonTemplateFromFileAndCheckActorTypeTemplateCountAsync()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(testFileName));
            Assert.IsTrue(template.ActorTemplates.Count == 1);
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
