using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseRegistry : IService
    {
        Task RegisterUniverseAsync(List<ActorTemplate> actorTemplates);
        Task RegisterActorAsync(string id, string type);
        Task UnregisterActorAsync(string id);
        Task UnregisterUniverseAsync();
    }
}
