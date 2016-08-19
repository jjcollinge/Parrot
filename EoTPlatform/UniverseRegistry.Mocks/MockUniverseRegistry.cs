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

        public Task<Dictionary<string, UniverseDescriptor>> GetUniversesAsync()
        {
            return Task.FromResult(new Dictionary<string, UniverseDescriptor>());
        }

        public Task RegisterUniverseAsync(UniverseDescriptor universe)
        {
            return Task.FromResult(true);
        }
    }
}
