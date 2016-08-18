using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUniverseTemplateLoader : IService
    {
        Task<UniverseTemplate> LoadTemplateFromFileAsync(string templateFilePath);
    }
}
