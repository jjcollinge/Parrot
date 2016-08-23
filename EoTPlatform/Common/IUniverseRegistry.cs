using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseRegistry : IService
    {
        Task RegisterUniverseAsync(UniverseDescriptor universe);
        Task DeregisterUniverseAsync(string id);

        Task<Dictionary<string, UniverseDescriptor>> GetUniversesAsync();

        Task<UniverseDescriptor> GetUniverseAsync(string universeId);
    }
}
