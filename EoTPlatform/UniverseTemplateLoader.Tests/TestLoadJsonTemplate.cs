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
        [TestMethod]
        public async Task LoadJsonTemplateFromFileAsync()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadTemplateFromFileAsync($"{GetTemplatePath("testTemplate.json")}");
            Assert.IsTrue(template.Id == "0");
            Assert.IsNotNull(template.ActorTemplates);
        }

        [TestMethod]
        public async Task LoadJsonTemplateFromFileAndCheckTemplateName()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadTemplateFromFileAsync($"{GetTemplatePath("testTemplate.json")}");
            Assert.IsTrue(template.Id == "0");
        }

        [TestMethod]
        public async Task LoadJsonTemplateFromFileAndCheckActorTypeTemplateCountAsync()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadTemplateFromFileAsync($"{GetTemplatePath("testTemplate.json")}");
            Assert.IsTrue(template.ActorTemplates.Count == 2);
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
