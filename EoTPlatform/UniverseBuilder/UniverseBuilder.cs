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
using Microsoft.ServiceFabric.Actors.Client;
using UniverseActor.Interfaces;
using Microsoft.ServiceFabric.Actors;

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

        public async Task<UniverseDefinition> BuildUniverseAsync(string dataSourceFilePath, UniverseTemplate template)
        {
            if (template == null)
                return null;

            // Create empty universe definition
            var universeDefinition = new UniverseDefinition();

            // Create the universe actors
            var actorIds = await CreateUniverseActorsAsync(template.ActorTemplates);

            // Create the services needed for the universe
            await CreateUniverseActorRegistryAsync(actorIds, universeDefinition);
            await CreateUniverseSchedulerAsync(dataSourceFilePath, universeDefinition);

            //TODO: Compile custom plugin assembly files

            // Return description of all the universe services and metadata 
            return universeDefinition;
        }

        private async Task<IDictionary<string, ActorId>> CreateUniverseActorsAsync(List<ActorTemplate> actorTemplates)
        {
            var actorIds = new Dictionary<string, ActorId>();

            // Create and initialise an actor for each actor defined in the template
            Parallel.ForEach<ActorTemplate>(actorTemplates, async (actorTemplate) => {
                var actorId = new ActorId(actorTemplate.Id);
                var actor = ActorProxy.Create<IUniverseActor>(actorId);
                await actor.Initialise(actorTemplate);
                actorIds.Add(actorTemplate.Id, actorId);
            });

            return actorIds;
        }

        private async Task CreateUniverseSchedulerAsync(string dataSourceFilePath, UniverseDefinition universeDefinition)
        {
            // Create a new scheduler service
            var appName = await platform.GetServiceContextApplicationNameAsync();
            var randomPrefix = new Random().Next(0, 99999).ToString();
            var serviceAddress = $"{appName}/UniverseScheduler{randomPrefix}";
            var serviceName = new Uri(serviceAddress);
            var serviceTypeName = "UniverseSchedulerType";

            await platform.BuildServiceAsync(appName, serviceName, serviceTypeName, ServiceContextTypes.Stateless);

            // Start the event stream
            var universeScheduler = proxyFactory.CreateUniverseScheduler(serviceName);
            await universeScheduler.StartAsync(dataSourceFilePath, universeDefinition);

            universeDefinition.AddServiceEndpoints(serviceTypeName, new List<string> { serviceAddress });
        }

        private async Task CreateUniverseActorRegistryAsync(IDictionary<string, ActorId> actorIds, UniverseDefinition universeDefinition)
        {
            // Create a new registry service
            var appName = await platform.GetServiceContextApplicationNameAsync();
            var randomPrefix = new Random().Next(0, 99999).ToString();
            var serviceAddress = $"{appName}/UniverseActorRegistry{randomPrefix}";
            var serviceName = new Uri(serviceAddress);
            var serviceTypeName = "UniverseActorRegistryType";

            await platform.BuildServiceAsync(appName, serviceName, serviceTypeName, ServiceContextTypes.Stateful);

            // Store each actor in the universes id in the registry
            var universeActorRegistry = proxyFactory.CreateUniverseActorRegistryServiceProxy(serviceName);
            await universeActorRegistry.RegisterUniverseActorsAsync(actorIds);

            universeDefinition.AddServiceEndpoints(serviceTypeName, new List<string> { serviceAddress });
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context))
            };
        }

    }
}
