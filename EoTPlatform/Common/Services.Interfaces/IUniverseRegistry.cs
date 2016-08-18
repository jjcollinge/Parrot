using Common.Services.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Interfaces
{
    public interface IUniverseRegistry : IService
    {
        Task RegisterUniverseAsync(List<ActorTemplate> actorTemplates);
        Task RegisterActorAsync(string id, string type);
        Task UnregisterActorAsync(string id);
        Task UnregisterUniverseAsync();
    }
}
