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
        public Task RegisterActorAsync(string id, string type)
        {

            return Task.FromResult(true);
        }

        public Task RegisterUniverseAsync(List<ActorTemplate> actorTemplates)
        {
            return Task.FromResult(true);
        }

        public Task UnregisterActorAsync(string id)
        {
            return Task.FromResult(true);
        }

        public Task UnregisterUniverseAsync()
        {
            return Task.FromResult(true);
        }
    }
}
