using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Models;
using Microsoft.ServiceFabric.Actors.Client;
using UniverseActor.Interfaces;
using System.Text.RegularExpressions;
using Common.Interfaces;
using System.IO;
using Microsoft.ServiceFabric.Actors;
using System.Threading;

namespace UniverseScheduler
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class UniverseScheduler : StatelessService, IUniverseScheduler
    {
        // Private
        private IServiceProxyFactory factory;
        private IPlatformAbstraction platform;
        private IDictionary<UniverseEvent, Timer> timers;
        private IEventStreamFileLoader fileLoader;

        // Public
        public Queue<UniverseEvent> eventStream { get; private set; }
        public IDictionary<string, ActorId> actorMap { get; private set; }

        public UniverseScheduler(StatelessServiceContext context, IServiceProxyFactory factory, IPlatformAbstraction platform, IEventStreamFileLoader eventStreamFileLoader)
            : base(context)
        {
            this.factory = factory;
            this.platform = platform;
            timers = new Dictionary<UniverseEvent, Timer>();
            fileLoader = eventStreamFileLoader;
        }

        /// <summary>
        /// Initialise the event stream to replay.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        public async Task SetupAsync(string eventStreamFilePath, UniverseDefinition universeDefinition)
        {
            await SetupEventStreamAsync(eventStreamFilePath);
            await SetupActorMap(universeDefinition);
        }

        /// <summary>
        /// Stop the universe event stream.
        /// </summary>
        /// <returns></returns>
        public Task StopAsync()
        {
            foreach(var timer in timers.Values)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Setup event stream and begin streaming.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        public Task StartAsync()
        {
            var startDelay = TimeSpan.FromMilliseconds(5000);
            var baseTime = DateTime.MinValue;

            timers.Clear();

            // Assume events are in ascending order
            while(eventStream.Count > 0)
            {
                var evt = eventStream.Dequeue();

                // Base time is the first event in the list
                if (baseTime == DateTime.MinValue)
                    baseTime = evt.OriginalTimeStamp;

                TimeSpan timeDelta = evt.OriginalTimeStamp - baseTime + startDelay;

                if (timeDelta < TimeSpan.Zero)
                {
                    return Task.FromResult(true);
                }
                timers.Add(evt, new Timer(e =>
                {
                    DispatchEvent((UniverseEvent)e);
                }, evt, timeDelta, Timeout.InfiniteTimeSpan));
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Loads an event stream from file.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <returns></returns>
        private async Task SetupEventStreamAsync(string eventStreamFilePath)
        {
            eventStream = fileLoader.ReadEventStreamFromFile(eventStreamFilePath);
        }

        /// <summary>
        /// Loads data from universe services that is required.
        /// </summary>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        private async Task SetupActorMap(UniverseDefinition universeDefinition)
        {
            // Assumes desired endpoint is at index 0
            var registryEndpoint = universeDefinition.ServiceEndpoints["UniverseActorRegistryType"][0];
            var registry = factory.CreateUniverseActorRegistryServiceProxy(new Uri(registryEndpoint));

            // TODO: Local caching with periodic refreshes - at present assumes fixed registry
            actorMap = await registry.GetAllRegisteredUniverseActorsAsync();
        }

        /// <summary>
        /// Called to send an event to the appropriate actor
        /// </summary>
        /// <param name="evt"></param>
        private async void DispatchEvent(UniverseEvent evt)
        {
            // Send event to appropriate actor
            var actorId = actorMap[evt.ActorId];
            var applicationName = await platform.GetServiceContextApplicationNameAsync();
            var actor = await platform.CreateUniverseActorProxyAsync(actorId, new Uri($"{applicationName}/UniverseActorService"));
            await actor.ProcessEventAsync(evt);
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
