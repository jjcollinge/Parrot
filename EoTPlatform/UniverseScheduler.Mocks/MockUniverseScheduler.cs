using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseScheduler.Mocks
{
    public class MockUniverseScheduler : IUniverseScheduler
    {
        public Task LoadEventStreamAsync(string eventStreamFilePath)
        {
            return Task.FromResult(true);
        }

        public Task LoadUniverseDefinitionAsync(UniverseDefinition universeDefinition)
        {
            return Task.FromResult(true);
        }

        public Task<bool> PauseAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> StartAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> StopAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> UnpauseAsync()
        {
            return Task.FromResult(true);
        }
    }
}
