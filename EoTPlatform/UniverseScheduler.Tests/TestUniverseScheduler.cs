﻿using System;
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
        [ExpectedException(typeof(FileNotFoundException), "File does not exist at given file path.")]
        public async Task TestSetupAsyncWithInvalidFilePath()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseDataFail.csv";

            UniverseDefinition universeDefinition = new UniverseDefinition();

            await scheduler.SetupAsync(universeEventStreamFilePath, universeDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "Service keys incorrect for required endpoints")]
        public async Task TestSetupAsyncWithUniverseDefinitionMissingEndpoints()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            UniverseDefinition universeDefinition = new UniverseDefinition();

            await scheduler.SetupAsync(universeEventStreamFilePath, universeDefinition);
        }

        [TestMethod]
        public async Task TestSetupAsyncSuccess()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            UniverseDefinition universeDefinition = new UniverseDefinition();
            universeDefinition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.SetupAsync(universeEventStreamFilePath, universeDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ensure the event stream has been loaded and the universe definition has been provided.")]
        public async Task TestSetupAsyncWithNullEventStream()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            UniverseDefinition definition = new UniverseDefinition();
            definition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.SetupAsync(null, definition);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ensure the event stream has been loaded and the universe definition has been provided.")]
        public async Task TestSetupAsyncWithNullUniverseDefinition()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            await scheduler.SetupAsync(universeEventStreamFilePath, null);
        }

        [TestMethod]
        public async Task TestStartAsyncSuccess()
        {
            var scheduler = new UniverseScheduler(null, new MockServiceProxyFactory(), new MockPlatformAbstraction());

            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            UniverseDefinition universeDefinition = new UniverseDefinition();
            universeDefinition.AddServiceEndpoints("UniverseActorRegistryType", new List<string> { "localhost:80" });

            await scheduler.SetupAsync(universeEventStreamFilePath, universeDefinition);

            await scheduler.StartAsync();
        }
    }
}
