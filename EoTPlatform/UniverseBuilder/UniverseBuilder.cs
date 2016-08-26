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

        private static string applicationName;
        private static string randomPrefix;

        public UniverseBuilder(StatelessServiceContext context, IPlatformAbstraction platform, IServiceProxyFactory proxyFactory)
            : base(context)
        {
            this.platform = platform;
            this.proxyFactory = proxyFactory;
        }

        /// <summary>
        /// Build a universe from a provided template description and start streaming events in from a provided data source file.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public async Task<UniverseDefinition> BuildUniverseAsync(string eventStreamFilePath, UniverseTemplate template)
        {
            if (template == null)
                return null;

            applicationName = await platform.GetServiceContextApplicationNameAsync();
            randomPrefix = new Random().Next(0, 99999).ToString();

            // Create empty universe definition
            var universeDefinition = new UniverseDefinition();

            // Create the universe actors
            var actorIds = await CreateUniverseActorsAsync(template.ActorTemplates);

            if (actorIds.Count > 0)
            {
                // Create the services needed for the universe
                await CreateUniverseActorRegistryAsync(actorIds, universeDefinition);
                await CreateUniverseSchedulerAsync(eventStreamFilePath, universeDefinition);

                //TODO: Compile custom plugin assembly files

                // Return description of all the universe services and metadata 
                universeDefinition.Status = UniverseStatus.Running;
            }
            else
            {
                universeDefinition.Status = UniverseStatus.Failed;
            }

            return universeDefinition;
        }

        /// <summary>
        /// Create universe actors to represent the provided actors in the template file.
        /// </summary>
        /// <param name="actorTemplates"></param>
        /// <returns></returns>
        private async Task<IDictionary<string, ActorId>> CreateUniverseActorsAsync(List<ActorTemplate> actorTemplates)
        {
            var actorIds = new Dictionary<string, ActorId>();

            applicationName = await platform.GetServiceContextApplicationNameAsync();
            // Create and initialise an actor for each actor defined in the template
            foreach(var template in actorTemplates)
            {
                var actorId = await platform.GetActorIdAsync(template.Id);
                var actor = await platform.CreateUniverseActorProxyAsync(actorId, new Uri($"{applicationName}/UniverseActorService"));
                await actor.SetupAsync(template);
                actorIds.Add(template.Id, actorId);
            }

            return actorIds;
        }

        /// <summary>
        /// Create a scheduler to stream events into the actor network.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        private async Task CreateUniverseSchedulerAsync(string eventStreamFilePath, UniverseDefinition universeDefinition)
        {
            // Create a new scheduler service
            var serviceBaseName = "UniverseScheduler";
            var serviceAddress = GetServiceAddress(serviceBaseName);
            var serviceType = GetServiceType(serviceBaseName);
            var serviceUri = new Uri(serviceAddress);

            await platform.BuildServiceAsync(applicationName, serviceUri, serviceType, ServiceContextTypes.Stateless);

            // Start the event stream
            var universeScheduler = proxyFactory.CreateUniverseScheduler(serviceUri);
            await universeScheduler.LoadEventStreamAsync(eventStreamFilePath);
            await universeScheduler.LoadUniverseDefinitionAsync(universeDefinition);
            await universeScheduler.StartAsync();

            universeDefinition.AddServiceEndpoints(serviceType, new List<string> { serviceAddress });
        }

        /// <summary>
        /// Create a registry to map external Ids to internal actor Ids.
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        private async Task CreateUniverseActorRegistryAsync(IDictionary<string, ActorId> actorIds, UniverseDefinition universeDefinition)
        {
            // Create a new registry service
            var serviceBaseName = "UniverseActorRegistry";
            var serviceAddress = GetServiceAddress(serviceBaseName);
            var serviceType = GetServiceType(serviceBaseName);
            var serviceUri = new Uri(serviceAddress);

            await platform.BuildServiceAsync(applicationName, serviceUri, serviceType, ServiceContextTypes.Stateful);

            // Store each actor in the universes id in the registry
            var universeActorRegistry = proxyFactory.CreateUniverseActorRegistryServiceProxy(serviceUri);
            await universeActorRegistry.RegisterUniverseActorsAsync(actorIds);

            universeDefinition.AddServiceEndpoints(serviceType, new List<string> { serviceAddress });
        }

        /// <summary>
        /// Compile a service's address based on it's base name.
        /// </summary>
        /// <param name="serviceBaseName"></param>
        /// <returns></returns>
        private string GetServiceAddress(string serviceBaseName)
        {
            return $"{applicationName}/{serviceBaseName}{randomPrefix}";
        }

        /// <summary>
        /// Compile a service's type name based on it's base name.
        /// </summary>
        /// <param name="serviceBaseName"></param>
        /// <returns></returns>
        private string GetServiceType(string serviceBaseName)
        {
            return $"{serviceBaseName}Type";
        }

        /// <summary>
        /// Create service listener endpoints
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context))
            };
        }

    }
}
