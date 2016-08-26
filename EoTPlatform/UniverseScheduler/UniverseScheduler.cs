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

        // Public
        public Queue<DataEvent> eventStream { get; private set; }
        public IDictionary<string, ActorId> actorMap { get; private set; }

        public UniverseScheduler(StatelessServiceContext context, IServiceProxyFactory factory, IPlatformAbstraction platform)
            : base(context)
        {
            this.factory = factory;
            this.platform = platform;
        }

        /// <summary>
        /// Loads an event stream from file.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <returns></returns>
        public async Task LoadEventStreamAsync(string eventStreamFilePath)
        {
            IList<CsvRow> rows = LoadCSVRowsFromCSVFile(eventStreamFilePath);
            eventStream = ConvertCSVRowsToDataEventStream(rows);
        }

        /// <summary>
        /// Loads data from universe services that is required.
        /// </summary>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        public async Task LoadUniverseDefinitionAsync(UniverseDefinition universeDefinition)
        {
            // Assumes desired endpoint is at index 0
            var registryEndpoint = universeDefinition.ServiceEndpoints["UniverseActorRegistryType"][0];
            var registry = factory.CreateUniverseActorRegistryServiceProxy(new Uri(registryEndpoint));

            // TODO: Local caching with periodic refreshes - at present assumes fixed registry
            actorMap = await registry.GetRegisteredActorsAsync();
        }

        /// <summary>
        /// Pause event streaming.
        /// </summary>
        /// <returns></returns>
        public Task<bool> PauseAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unpause event streaming.
        /// </summary>
        /// <returns></returns>
        public Task<bool> UnpauseAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop the universe event stream.
        /// </summary>
        /// <returns></returns>
        public Task<bool> StopAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Setup event stream and begin streaming.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <param name="universeDefinition"></param>
        /// <returns></returns>
        public async Task<bool> StartAsync()
        {
            if (eventStream == null || actorMap == null)
                throw new InvalidOperationException("Ensure the event stream has been loaded and the universe definition has been provided.");

            var started = false;

            // Stream events [Will be refactored heavily to reflect proper scheduler]
            while (eventStream.Count > 0)
            {
                // Take event of the queue to process
                var ev = eventStream.Dequeue();

                // Wait until the right time
                await Task.Delay(ev.TimeOffset);

                // Sned event to appropriate actor (Batching?)
                var actorId = actorMap[ev.TargetId];
                var applicationName = await platform.GetServiceContextApplicationNameAsync();
                var actor = await platform.CreateUniverseActorProxyAsync(actorId, new Uri($"{applicationName}/UniverseActorService"));
                await actor.SendMessageAsync(ev.Payload);

                started = true;
            }

            return started;
        }

        /// <summary>
        /// Parse the CSV rows and generate an event stream.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private Queue<DataEvent> ConvertCSVRowsToDataEventStream(IList<CsvRow> rows)
        {
            if (rows == null)
                throw new NullReferenceException();

            // Assumes row is in format: [TimeStamp, Id, PropertyA, PropertyB...]

            var events = new Queue<DataEvent>();

            // Assume data is ordered
            DateTime baseTime = DateTime.MinValue;

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];

                // Parse time
                var ev = new DataEvent();
                var dateTimeStr = Regex.Replace(row[0].Trim(), "[^0-9/:]", " ");
                var originalTime = DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy HH:mm:ss", null);

                if (baseTime == DateTime.MinValue)
                    baseTime = originalTime;

                var deltaTimeSpan = originalTime - baseTime;
                ev.TimeOffset = deltaTimeSpan;

                ev.TargetId = row[1];

                // Parse payload
                for (int j = 2; j < row.Count; j++)
                {
                    ev.Payload += row[j] + " ";
                }

                // Remove trailing whitespace
                ev.Payload.Trim();
                events.Enqueue(ev);
            }
            return events;
        }

        /// <summary>
        /// Load an in memory representation of data in a CSV file.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <returns></returns>
        private IList<CsvRow> LoadCSVRowsFromCSVFile(string eventStreamFilePath)
        {
            if (eventStreamFilePath == null)
                throw new NullReferenceException();

            IList<CsvRow> rows = new List<CsvRow>();

            try
            {
                // Read file into memory
                using (var reader = new CsvFileReader(eventStreamFilePath))
                {
                    CsvRow row = new CsvRow();
                    while (reader.ReadRow(row))
                    {
                        ServiceEventSource.Current.ServiceMessage(this, $"Read CSV row '{row}'");
                        rows.Add(row);
                    }
                }
            }
            catch (IOException ex)
            {
                ServiceEventSource.Current.ServiceMessage(this, $"Failed to read CSV file at path '{eventStreamFilePath}' with exception '{ex}'");
                throw ex;
            }

            return rows;
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
