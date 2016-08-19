using System;
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

        public async Task RegisterUniverseAsync(UniverseDescriptor universe)
        {
            var universes = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UniverseDescriptor>>("universes");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await universes.SetAsync(tx, Guid.NewGuid().ToString(), universe);
                await tx.CommitAsync();
            }
        }

        public async Task DeregisterUniverseAsync(string id)
        {
            var universes = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UniverseDescriptor>>("universes");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await universes.TryRemoveAsync(tx, id); //TODO: Handle failure
                await tx.CommitAsync();
            }
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
