using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseTemplateBuilder : IService
    {
        Task<string> BuildUniverseTemplateFromEndpointAsync(string sourceUrl);
        Task<string> BuildUniverseTemplateFromFileAsync(string inputFilePath);
        Task BuildUniverseTemplateFromFileAsync(string inputFilePath, string outputFilePath);
    }
}
