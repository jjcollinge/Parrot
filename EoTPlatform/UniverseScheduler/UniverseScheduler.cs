using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace UniverseScheduler
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class UniverseScheduler : StatelessService, IUniverseScheduler
    {
        private string sourceDataFilePath;

        public UniverseScheduler(StatelessServiceContext context)
            : base(context)
        { }

        public Task PauseAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnpauseAsync()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(string dataFilePath, int delayInMs)
        { 
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context))
            };
        }
    }
}
