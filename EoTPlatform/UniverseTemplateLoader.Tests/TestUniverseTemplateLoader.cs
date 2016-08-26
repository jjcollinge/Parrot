using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Fabric;
using System.Threading.Tasks;
using System.IO;

namespace UniverseTemplateLoader.Tests
{
    [TestClass]
    public class TestUniverseTemplateLoader
    {
        private static string testTemplateId = "universe123124";
        private static string testFileName = "testTemplate.json";
        private static string invalidFileName = "testTemplateInvalid.json";
        private static string UnsupportedFileType = "testTemplate.jar";
        private static string invalidFileContent = "invalidTemplate.json";

        [TestMethod]
        public async Task TestLoadUniversalTemplateFromFileAsyncSuccess()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(testFileName));
            Assert.IsTrue(template.Id == testTemplateId);
            Assert.IsNotNull(template.ActorTemplates);
            Assert.IsTrue(template.ActorTemplates.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "Invalid file path provided.")]
        public async Task TestLoadUniversalTemplateFromFileAsyncWithInvalidFilePath()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(invalidFileName));
        }

        [TestMethod]
        [ExpectedException(typeof(IOException), "Unsupported file type.")]
        public async Task TestLoadUniversalTemplateFromFileAsyncWithNonSupportedFileType()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(UnsupportedFileType));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException), "Unsupported file type.")]
        public async Task TestLoadUniversalTemplateFromFileAsyncWithInvalidFileContent()
        {
            UniverseTemplateLoader loader = new UniverseTemplateLoader(null);
            var template = await loader.LoadUniversalTemplateFromFileAsync(GetTemplatePath(invalidFileContent));
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
