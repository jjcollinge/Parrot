using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Mocks;
using Common.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace UniverseScheduler.Tests
{
    [TestClass]
    public class TestLoadEventAsyncStream
    {
        [TestMethod]
        public async Task TestLoadEventStreamAsyncSuccess()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            await scheduler.LoadEventStreamAsync(universeEventStreamFilePath);

            Assert.IsTrue(scheduler.eventStream?.Count == 10);
        }

        [TestMethod]
        public async Task TestLoadEventStreamAsyncFail()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            await scheduler.LoadEventStreamAsync(universeEventStreamFilePath);

            Assert.IsFalse(scheduler.eventStream?.Count == 5);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "File does not exist at given file path.")]
        public async Task TestLoadEventStreamAsyncWithIncorrectFilePath()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseDataFail.csv";

            await scheduler.LoadEventStreamAsync(universeEventStreamFilePath);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Null parameters provided.")]
        public async Task TestLoadEventStreamAsyncWithNullFilePath()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            await scheduler.LoadEventStreamAsync(null);
        }

        [TestMethod]
        public async Task TestLoadUniverseDefinitionAsyncSuccess()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            UniverseDefinition definition = new UniverseDefinition();
            definition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.LoadUniverseDefinitionAsync(definition);

            Assert.IsTrue(scheduler.actorMap.ContainsKey("mock"));
        }

        [TestMethod]
        public async Task TestLoadUniverseDefinitionAsyncFail()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            UniverseDefinition definition = new UniverseDefinition();
            definition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.LoadUniverseDefinitionAsync(definition);

            Assert.IsFalse(scheduler.actorMap.ContainsKey("badger"));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "Service keys incorrect for required endpoints")]
        public async Task TestLoadUniverseDefinitionAsyncWithNoServiceEndpoints()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            UniverseDefinition definition = new UniverseDefinition();

            await scheduler.LoadUniverseDefinitionAsync(definition);
        }

        [TestMethod]
        public async Task TestStartAsyncSuccess()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            UniverseDefinition definition = new UniverseDefinition();
            definition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.LoadEventStreamAsync(universeEventStreamFilePath);
            await scheduler.LoadUniverseDefinitionAsync(definition);

            var success = await scheduler.StartAsync();

            Assert.IsTrue(success);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ensure the event stream has been loaded and the universe definition has been provided.")]
        public async Task TestStartAsyncWithoutLoadingEventStream()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            UniverseDefinition definition = new UniverseDefinition();
            definition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.LoadUniverseDefinitionAsync(definition);

            var success = await scheduler.StartAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ensure the event stream has been loaded and the universe definition has been provided.")]
        public async Task TestStartAsyncWithoutLoadingUniverseDefinition()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            UniverseDefinition definition = new UniverseDefinition();
            definition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.LoadUniverseDefinitionAsync(definition);

            var success = await scheduler.StartAsync();
        }
    }
}
