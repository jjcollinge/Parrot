using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseRegistry : IService
    {
        Task<bool> RegisterUniverseAsync(string id, UniverseDefinition universe);
        Task<bool> DeregisterUniverseAsync(string id);
        Task<IDictionary<string, UniverseDefinition>> GetAllUniverseAsync();
        Task<KeyValuePair<string, UniverseDefinition>> GetUniverseAsync(string universeId);
    }
}
