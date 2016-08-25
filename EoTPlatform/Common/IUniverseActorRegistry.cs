using Common.Models;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IUniverseActorRegistry : IService
    {
        Task RegisterUniverseActorAsync(string actorIdAsString, ActorId actorId);
        Task DeregisterUniverseActorAsync(string actorIdAsString);
        Task RegisterUniverseActorsAsync(IDictionary<string, ActorId> actorIds);
        Task DeregisterAllUniverseActorsAsync();
        Task<IDictionary<string, ActorId>> GetRegisteredActorsAsync();
        Task<KeyValuePair<string, ActorId>> GetRegisteredActorAsync(string actorIdAsString);
    }
}
