using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Interfaces;
using Common.Models;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Client;
using Common.Services;
using System.Threading;

namespace UniverseBuilder
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class UniverseBuilder : StatelessService, IUniverseBuilder
    {
        private IPlatformAbstraction platform;
        private IServiceProxyFactory proxyFactory;

        public UniverseBuilder(StatelessServiceContext context, IPlatformAbstraction platform, IServiceProxyFactory proxyFactory)
            : base(context)
        {
            this.platform = platform;
            this.proxyFactory = proxyFactory;
        }

        public async Task<UniverseDescriptor> BuildUniverseAsync(UniverseTemplate template)
        {
            if (template == null)
                return null;

            var universeServicesEndpoints = await CreateUniverseServicesAsync(template.ActorTemplates);

            // Rather than pass back all the actor ids here, pass back a reference to a stateful service endpoint which has been preloaded with them.
            var universeDescriptor = new UniverseDescriptor
            {
                ServiceEndpoints = universeServicesEndpoints
            };

            return universeDescriptor;
        }

        private async Task<Dictionary<string, List<string>>> CreateUniverseServicesAsync(List<ActorTemplate> actorTemplates)
        {
            var universeActorRegistryKVP = await CreateUniverseActorRegistryAsync(actorTemplates);

            return new Dictionary<string, List<string>>
            {
                {universeActorRegistryKVP.Key, universeActorRegistryKVP.Value}
            };
        }

        private async Task<KeyValuePair<string, List<string>>> CreateUniverseActorRegistryAsync(List<ActorTemplate> actorTemplates)
        {
            var appName = await platform.GetServiceContextApplicationNameAsync();
            var randomPrefix = new Random().Next(0, 99999).ToString();
            var serviceAddress = $"{appName}/UniverseActorRegistry{randomPrefix}";
            var serviceName = new Uri(serviceAddress);
            var serviceTypeName = "UniverseActorRegistryType";

            //TODO: Figure out how we should deal with port contention if running 'n' universes
            await platform.BuildServiceAsync(appName, serviceName, serviceTypeName, ServiceContextTypes.Stateful);

            var universeActorRegistry = proxyFactory.CreateUniverseActorRegistryServiceProxy(serviceName);
            await universeActorRegistry.RegisterUniverseActorListAsync(actorTemplates);

            return new KeyValuePair<string, List<string>>(serviceTypeName, new List<string> { serviceAddress });
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
