using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common.Services.Interfaces;
using Common.Services.Models;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using System.Fabric.Description;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace UniverseBuilder
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class UniverseBuilder : StatelessService, IUniverseBuilder
    {
        private IPlatformWrapper platform;
        private IServiceProxyFactory proxyFactory;

        public UniverseBuilder(StatelessServiceContext context, IPlatformWrapper platform, IServiceProxyFactory proxyFactory)
            : base(context)
        {
            this.platform = platform;
            this.proxyFactory = proxyFactory;
        }

        public async Task<UniverseDescriptor> BuildUniverseAsync(UniverseTemplate template)
        {
            if (template == null)
                return null;

            var universeServicesEndpoints = await CreateUniverseServices(template.ActorTemplates);

            // Rather than pass back all the actor ids here, pass back a reference to a stateful service endpoint which has been preloaded with them.
            var universeDescriptor = new UniverseDescriptor
            {
                ServiceEndpoints = universeServicesEndpoints
            };

            return universeDescriptor;
        }

        private async Task<Dictionary<string, Uri>> CreateUniverseServices(List<ActorTemplate> actorTemplates)
        {
            var appName = await platform.GetServiceContextApplicationNameAsync();
            var randomPrefix = new Random().Next(0, 99999).ToString();
            var serviceName = new Uri($"fabric:/{appName}/UniverseRegistry{randomPrefix}");
            var serviceTypeName = "UniverseRegistry";

            await platform.BuildServiceAsync(appName, serviceName, serviceTypeName);

            var universe = proxyFactory.CreateUniverseRegistryServiceProxy(serviceName);
            await universe.RegisterUniverseAsync(actorTemplates);

            return new Dictionary<string, Uri>
            {
                { serviceTypeName, serviceName }
            };
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
