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

        public Task PauseAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnpauseAsync()
        {
            throw new NotImplementedException();
        }

        public async Task StartAsync(string dataFilePath, UniverseDefinition universeDefinition)
        {
            IList<CsvRow> rows = new List<CsvRow>();

            // Read file into memory
            using (var reader = new CsvFileReader(dataFilePath))
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

            // Stream events
            Parallel.ForEach<DataStreamEvent>(eventStream, (evt) => {
                var actor = ActorProxy.Create<IUniverseActor>(actorIds[evt.TargetActorId]);
                actor.SendMessageAsync(evt.Payload);
            });
                
        }

        private Task<bool> ValidateEventStreamActorsAsync(object eventStream)
        {
            return Task.FromResult(true);
        }

        private List<DataStreamEvent> CalculateEventStream(IList<CsvRow> rows)
        {
            List<DataStreamEvent> events = new List<DataStreamEvent>();
            foreach (var row in rows)
            {
                var dataEvent = new DataStreamEvent();
                var dateTimeStr = Regex.Replace(row[0].Trim(), "[^0-9/:]", " ");
                dataEvent.DueTime = DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy HH:mm:ss", null);

                dataEvent.TargetActorId = row[1];

                for(int i = 2; i < row.Count; i++)
                {
                    dataEvent.Payload += $"{row[i]} ";
                }
                events.Add(dataEvent);
            }
            return events;
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
