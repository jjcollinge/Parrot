using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IUniverseScheduler : IService
    {
        Task LoadEventStreamAsync(string eventStreamFilePath);
        Task LoadUniverseDefinitionAsync(UniverseDefinition universeDefinition);
        Task<bool> StartAsync();
        Task<bool> StopAsync();
        Task<bool> PauseAsync();
        Task<bool> UnpauseAsync();
    }
}
