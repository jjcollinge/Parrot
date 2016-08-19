using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseTemplateBuilder : IService
    {
        Task<string> BuildUniverseTemplateFromEndpointAsync(string sourceUrl);
        Task<string> BuildUniverseTemplateFromCSVAsync(string inputFilePath);
        Task BuildUniverseTemplateFromCSVAndWriteToJsonFileAsync(string inputFilePath, string outputFilePath);
    }
}
