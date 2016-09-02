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
        public Task ClearAll()
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeregisterUniverseActorAsync(string id)
        {
            return Task.FromResult(true);
        }

        public Task<KeyValuePair<string, ActorId>> GetRegisteredUniverseActorAsync(string id)
        {
            return Task.FromResult(new KeyValuePair<string, ActorId>());
        }

        public async Task<IDictionary<string, ActorId>> GetAllRegisteredUniverseActorsAsync()
        {
            return new Dictionary<string, ActorId>{
                { "MOCK", ActorId.CreateRandom() }
            };
        }

        public Task<bool> RegisterUniverseActorAsync(string id, ActorId actorId)
        {
            return Task.FromResult(true);
        }

        public Task ClearAllAsync<ActorId>()
        {
            return Task.FromResult(true);
        }
    }
}
