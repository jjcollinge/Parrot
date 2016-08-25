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
        public Task PauseAsync()
        {
            return Task.FromResult(true);
        }

        public Task StartAsync(string dataFilePath, UniverseDefinition universe)
        {
            return Task.FromResult(true);
        }

        public Task StopAsync()
        {
            return Task.FromResult(true);
        }

        public Task UnpauseAsync()
        {
            return Task.FromResult(true);
        }
    }
}
