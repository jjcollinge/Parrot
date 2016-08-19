using Common.Models;
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
        Task RegisterUniverseActorAsync(ActorTemplate actor);
        Task DeregisterUniverseActorAsync(string actorId);
        Task RegisterUniverseActorListAsync(List<ActorTemplate> actorList);
        Task DeregisterAllUniverseActorsAsync();
    }
}
