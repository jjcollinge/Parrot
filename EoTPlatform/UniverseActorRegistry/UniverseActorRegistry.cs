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
using Microsoft.ServiceFabric.Actors;

namespace UniverseActorRegistry
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class UniverseActorRegistry : StatefulService, IUniverseActorRegistry
    {
        //TODO: Consider piggy backging the actor services
        // Change read/write behaviour to exception proof
        private static string actorDictionary = "actors";

        private Task<IReliableDictionary<string, ActorId>> _actorIds => this.StateManager.GetOrAddAsync<IReliableDictionary<string, ActorId>>(actorDictionary);

        public UniverseActorRegistry(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<IDictionary<string, ActorId>> GetRegisteredActorsAsync()
        {
            var actorIdsToReturn = new Dictionary<string, ActorId>();

            var actorIds = await _actorIds;

            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await actorIds.CreateEnumerableAsync(tx);
                var enumerator = enumerable.GetAsyncEnumerator();
                var cancelToken = new CancellationToken();
                while(enumerator.MoveNextAsync(cancelToken) != null)
                {
                    actorIdsToReturn.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
            }

            return actorIdsToReturn;
        }


        public async Task<KeyValuePair<string, ActorId>> GetRegisteredActorAsync(string actorIdAsString)
        {
            var actorId = new KeyValuePair<string, ActorId>();

            var actorIds = await _actorIds;

            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await actorIds.TryGetValueAsync(tx, actorIdAsString);

                if (result.HasValue)
                    actorId = new KeyValuePair<string, ActorId>(actorIdAsString, result.Value);
            }

            return actorId;
        }

        public async Task RegisterUniverseActorAsync(string actorIdAsString, ActorId actorId)
        {
            var actorIds = await _actorIds;

            using (var tx = this.StateManager.CreateTransaction())
            {
                var success = await actorIds.TryAddAsync(tx, actorIdAsString, actorId);

                if (success)
                    ServiceEventSource.Current.ServiceMessage(this, $"Successfully registered actor id {actorIdAsString}");
                else
                    ServiceEventSource.Current.ServiceMessage(this, $"Failed to registered actor id {actorIdAsString}");

                await tx.CommitAsync();
            }
        }

        public async Task RegisterUniverseActorsAsync(IDictionary<string, ActorId> actorIds)
        {
            foreach (var actorId in actorIds)
            {
                await RegisterUniverseActorAsync(actorId.Key, actorId.Value);
            }
        }

        public async Task DeregisterAllUniverseActorsAsync()
        {
            var actors = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, ActorId>>(actorDictionary);

            using (var tx = this.StateManager.CreateTransaction())
            {
                await actors.ClearAsync();
                await tx.CommitAsync();
            }
        }

        public async Task DeregisterUniverseActorAsync(string actorIdAsString)
        {
            var actors = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, ActorId>>("actors");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await actors.TryRemoveAsync(tx, actorIdAsString); //TODO: Handle failure
                await tx.CommitAsync();
            }
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

    }
}
