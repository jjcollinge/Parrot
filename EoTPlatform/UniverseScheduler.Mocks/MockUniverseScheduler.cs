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

        public Task SetupAsync(string eventStreamFilePath, UniverseDefinition universeDefinition)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            return Task.FromResult(true);
        }

        public Task StopAsync()
        {
            return Task.FromResult(true);
        }

    }
}
