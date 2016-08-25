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
using Microsoft.ServiceFabric.Actors;
using System.Text.RegularExpressions;
using Common.Services;
using Common.Interfaces;

namespace UniverseScheduler
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class UniverseScheduler : StatelessService, IUniverseScheduler
    {
        private IServiceProxyFactory factory;

        public UniverseScheduler(StatelessServiceContext context, IServiceProxyFactory factory)
            : base(context)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Pause event streaming.
        /// </summary>
        /// <returns></returns>
        public Task PauseAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unpause event streaming.
        /// </summary>
        /// <returns></returns>
        public Task UnpauseAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Setup event stream and begin streaming.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        public async Task StartAsync(string eventStreamFilePath, UniverseDefinition universeDefinition)
        {
            IList<CsvRow> rows = new List<CsvRow>();

            // Read file into memory
            using (var reader = new CsvFileReader(eventStreamFilePath))
            {
                rows = reader.ReadFile();
            }

            // Calculate event stream
            var eventStream = CalculateEventStream(rows);

            // Validate event stream actors match registry actors
            var valid = ValidateEventStreamActorsAsync(eventStream);

            // Assumes desired endpoint is at index 0
            var registryEndpoint = universeDefinition.ServiceEndpoints["UniverseActorRegistryType"][0];
            var registry = factory.CreateUniverseActorRegistryServiceProxy(new Uri(registryEndpoint));

            // TODO: Local caching with periodic refreshes
            var actorIds = await registry.GetRegisteredActorsAsync();

            // Stream events [Will be refactored heavily to reflect proper scheduler]
            foreach(var evt in eventStream)
            {
                // Wait until the right time
                await Task.Delay(evt.Key);

                var actorId = actorIds[evt.Value.TargetId];
                var actor = ActorProxy.Create<IUniverseActor>(actorId);
                await actor.SendMessageAsync(evt.Value.Payload);
            }
        }

        /// <summary>
        /// Validate actors in event stream source match the actors in the universe template
        /// </summary>
        /// <param name="eventStream"></param>
        /// <returns></returns>
        private Task<bool> ValidateEventStreamActorsAsync(object eventStream)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Parse the CSV rows and generate an event stream.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private List<KeyValuePair<TimeSpan, DataStreamEvent>> CalculateEventStream(IList<CsvRow> rows)
        {
            // Assumes row is in format: [TimeStamp, Id, PropertyA, PropertyB...]

            var events = new List<KeyValuePair<TimeSpan, DataStreamEvent>>();

            // Assume data is ordered
            DateTime baseTime = DateTime.MinValue;

            for(int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                
                // Parse time
                var evt = new DataStreamEvent();
                var dateTimeStr = Regex.Replace(row[0].Trim(), "[^0-9/:]", " ");
                var originalTime = DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy HH:mm:ss", null);

                if (baseTime == DateTime.MinValue)
                    baseTime = originalTime;

                var deltaTimeSpan = originalTime - baseTime;

                evt.TargetId = row[1];

                // Parse payload
                for (int j = 2; j < row.Count; j++)
                {
                    evt.Payload += $"{row[j]} ";
                }

                events.Add(new KeyValuePair<TimeSpan, DataStreamEvent>(deltaTimeSpan, evt));
            }
            return events;
        }

        /// <summary>
        /// Stop the universe event stream.
        /// </summary>
        /// <returns></returns>
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
