using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActorRegistry.Mock
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

        public Task RegisterUniverseActorAsync(ActorTemplate actor)
        {
            return Task.FromResult(true);
        }

        public Task RegisterUniverseActorListAsync(List<ActorTemplate> actorList)
        {
            return Task.FromResult(true);
        }
    }
}
