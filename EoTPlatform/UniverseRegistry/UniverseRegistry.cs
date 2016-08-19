﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Models;
using Common.Interfaces;

namespace UniverseRegistry
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class UniverseRegistry : StatefulService, IUniverseRegistry
    {
        public UniverseRegistry(StatefulServiceContext context)
            : base(context)
        { }

        public Task RegisterActorAsync(string id, string type)
        {
            return Task.FromResult(true);
        }

        public async Task RegisterUniverseAsync(List<ActorTemplate> actorTemplates)
        {
            var universe = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("universe");

            foreach (var actorTemplate in actorTemplates)
            {
                using (var tx = this.StateManager.CreateTransaction())
                {
                    await universe.SetAsync(tx, actorTemplate.Id, actorTemplate.Type);
                    await tx.CommitAsync();
                }
            }
        }

        public Task UnregisterActorAsync(string id)
        {
            return Task.FromResult(true);
        }

        public Task UnregisterUniverseAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see http://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }
    }
}
