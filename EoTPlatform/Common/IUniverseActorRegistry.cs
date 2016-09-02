using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverseActor.Interfaces;

namespace Common
{
    public interface IUniverseActorRegistry : IService
    {
        Task<bool> RegisterUniverseActorAsync(string id, ActorId actor);
        Task<bool> DeregisterUniverseActorAsync(string id);
        Task<IDictionary<string, ActorId>> GetAllRegisteredUniverseActorsAsync();
        Task<KeyValuePair<string, ActorId>> GetRegisteredUniverseActorAsync(string id);
        Task ClearAllAsync<ActorId>();
    }
}
