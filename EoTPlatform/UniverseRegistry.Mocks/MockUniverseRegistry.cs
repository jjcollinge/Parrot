using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseRegistry.Mocks
{
    public class MockUniverseRegistry : IUniverseRegistry
    {
        public Task<bool> DeregisterUniverseAsync(string id)
        {
            return Task.FromResult(true);
        }

        public Task<KeyValuePair<string, UniverseDefinition>> GetUniverseAsync(string universeId)
        {
            return Task.FromResult(new KeyValuePair<string, UniverseDefinition>("0", new UniverseDefinition()));
        }

        public async Task<IDictionary<string, UniverseDefinition>> GetAllUniverseAsync()
        {
            var universe = new Dictionary<string, UniverseDefinition>();
            return universe;
        }

        public Task<bool> RegisterUniverseAsync(string id, UniverseDefinition universe)
        {
            return Task.FromResult(true);
        }
    }
}
