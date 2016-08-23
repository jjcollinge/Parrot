using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseWebApi.Services
{
    public interface IUniverseManagementService
    {
        Task CreateUniverseAsync(string universeTemplateFilePath);
        Task PauseUniverseAsync(string universeId);
        Task<Dictionary<string, UniverseDescriptor>> GetUniverseDescriptorsAsync();
        Task<UniverseDescriptor> GetUniverseDescriptorAsync(string universeId);
        Task DeleteUniverseAsync(string universeId);
        Task DeleteAllUniversesAsync();
    }
}
