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
        Task SetupAsync(string eventStreamFilePath, UniverseDefinition universeDefinition);
        Task StartAsync();
        Task StopAsync();
    }
}
