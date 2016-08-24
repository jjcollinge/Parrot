﻿using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IUniverseScheduler : IService
    {
        Task StartAsync(string dataFilePath, int delayInMs);
        Task StopAsync();
        Task PauseAsync();
        Task UnpauseAsync();
    }
}
