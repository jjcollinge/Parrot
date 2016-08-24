using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseBuilder : IService
    {
        /**
         * Given a data file and a template this method will build the necessary infrastruture for a universe
         **/
        Task<UniverseDescriptor> BuildUniverseAsync(string dataSourceFilePath, UniverseTemplate template);
    }
}
