using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common;
using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace UniverseActorRegistry
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class UniverseActorRegistry : StatefulService, IUniverseActorRegistry
    {
        //TODO: Consider piggy backging the actor services

        public UniverseActorRegistry(StatefulServiceContext context)
            : base(context)
        { }

        public async Task RegisterUniverseActorAsync(ActorTemplate actor)
        {
            var actors = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("actors");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await actors.SetAsync(tx, actor.Id, actor.Type);
                await tx.CommitAsync();
            }
        }

        public async Task RegisterUniverseActorListAsync(List<ActorTemplate> actorList)
        {
            foreach(var actor in actorList)
            {
                await RegisterUniverseActorAsync(actor);
            }
        }

        public async Task DeregisterAllUniverseActorsAsync()
        {
            var actors = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("actors");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await actors.ClearAsync();
                await tx.CommitAsync();
            }
        }

        public async Task DeregisterUniverseActorAsync(string actorId)
        {
            var actors = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("actors");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await actors.TryRemoveAsync(tx, actorId); //TODO: Handle failure
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
