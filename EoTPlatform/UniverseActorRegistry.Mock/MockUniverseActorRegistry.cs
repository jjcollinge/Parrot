using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace UniverseActorRegistry.Mocks
{
    public class MockUniverseActorRegistry : IUniverseActorRegistry
    {
        public Task DeregisterAllUniverseActorsAsync()
        {
            return Task.FromResult(true);
        }

        public Task DeregisterUniverseActorAsync(string actorId)
        {
            return Task.FromResult(true);
        }

        public Task<KeyValuePair<string, ActorId>> GetRegisteredActorAsync(string actorIdAsString)
        {
            return Task.FromResult(new KeyValuePair<string, ActorId>());
        }

        public async Task<IDictionary<string, ActorId>> GetRegisteredActorsAsync()
        {
            return new Dictionary<string, ActorId>();
        }

        public Task RegisterUniverseActorAsync(string actorIdAsString, ActorId actorId)
        {
            return Task.FromResult(true);
        }

        public Task RegisterUniverseActorsAsync(IDictionary<string, ActorId> actorIds)
        {
            return Task.FromResult(true);
        }
    }
}
