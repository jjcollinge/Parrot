using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseBuilder : IService
    {
        Task<UniverseDescriptor> BuildUniverseAsync(UniverseTemplate template);
    }
}
