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
    public sealed class UniverseRegistry : StatefulService, IUniverseRegistry
    {
        //TODO: Change read/write behaviour to exception proof

        public UniverseRegistry(StatefulServiceContext context)
            : base(context)
        { }

        /**
         * Iterates through each stored universe and compiles a dictionary of ids with descriptors
         **/
        public async Task<Dictionary<string, UniverseDefinition>> GetUniversesAsync()
        {
            var universes = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UniverseDefinition>>("universes");
            var universesDictionary = new Dictionary<string, UniverseDefinition>();

            //TODO: Should use immutable types
            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await universes.CreateEnumerableAsync(tx);
                var enumerator = enumerable.GetAsyncEnumerator();

                var ct = new CancellationToken();
                while (await enumerator.MoveNextAsync(ct))
                {
                    universesDictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
            }

            return universesDictionary;
        }

        /**
         * Registers a new universe descriptor in the universe registry
         **/
        public async Task RegisterUniverseAsync(UniverseDefinition universe)
        {
            var universes = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UniverseDefinition>>("universes");

            using (var tx = this.StateManager.CreateTransaction())
            {
                var alreadyExists = await universes.ContainsKeyAsync(tx, universe.Id);
                if (!alreadyExists)
                {
                    await universes.AddAsync(tx, universe.Id, universe);
                    await tx.CommitAsync();
                }
            }
        }

        /**
         * Deregisters a universe from the universe registry
         **/
        public async Task DeregisterUniverseAsync(string id)
        {
            var universes = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UniverseDefinition>>("universes");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await universes.TryRemoveAsync(tx, id); //TODO: Handle failure
                await tx.CommitAsync();
            }
        }

        /**
         * Grabs a specific universe descriptor from the universe registry provided it exists
         **/
        public async Task<UniverseDefinition> GetUniverseAsync(string universeId)
        {
            var universes = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UniverseDefinition>>("universes");

            UniverseDefinition descriptor = null;
            using (var tx = this.StateManager.CreateTransaction())
            {
                var res = await universes.TryGetValueAsync(tx, universeId);
                if (res.HasValue)
                    descriptor = res.Value;
            }
            return descriptor;
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
