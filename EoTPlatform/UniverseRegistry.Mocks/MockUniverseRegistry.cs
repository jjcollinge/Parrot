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
        public Task DeregisterUniverseAsync(string id)
        {
            return Task.FromResult(true);
        }

        public Task<UniverseDefinition> GetUniverseAsync(string universeId)
        {
            return Task.FromResult(new UniverseDefinition());
        }

        public Task<Dictionary<string, UniverseDefinition>> GetUniversesAsync()
        {
            return Task.FromResult(new Dictionary<string, UniverseDefinition>());
        }

        public Task RegisterUniverseAsync(UniverseDefinition universe)
        {
            return Task.FromResult(true);
        }
    }
}
