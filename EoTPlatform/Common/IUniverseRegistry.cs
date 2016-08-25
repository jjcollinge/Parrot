using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseRegistry : IService
    {
        Task RegisterUniverseAsync(UniverseDefinition universe);
        Task DeregisterUniverseAsync(string id);

        Task<Dictionary<string, UniverseDefinition>> GetUniversesAsync();

        Task<UniverseDefinition> GetUniverseAsync(string universeId);
    }
}
