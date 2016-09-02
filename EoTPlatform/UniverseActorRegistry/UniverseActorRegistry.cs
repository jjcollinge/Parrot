using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Services;
using Common;
using System;
using System.Threading.Tasks;
using UniverseActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Common.Interfaces;
using Microsoft.ServiceFabric.Data;

namespace UniverseActorRegistry
{
    /// <summary>
    /// Stores entries that map an external device id to an internal actor id
    /// </summary>
    public sealed class UniverseActorRegistry : StatefulService, IUniverseActorRegistry
    {
        private IRegistry registry;

        public UniverseActorRegistry(StatefulServiceContext context, IReliableStateManagerReplica stateManager)
            : base(context, stateManager)
        {
            this.registry = new Registry(stateManager);
        }

        /// <summary>
        /// Clears all stored universe actor ids
        /// </summary>
        /// <typeparam name="ActorId"></typeparam>
        /// <returns></returns>
        public async Task ClearAllAsync<ActorId>()
        {
            await registry.ClearAllAsync<ActorId>();
        }

        /// <summary>
        /// Removes a single universe actor id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeregisterUniverseActorAsync(string id)
        {
            var success = await registry.DeregisterAsync<ActorId>(id);
            return success;
        }

        /// <summary>
        /// Gets all the registered universe actor ids
        /// </summary>
        /// <returns></returns>
        public async Task<IDictionary<string, ActorId>> GetAllRegisteredUniverseActorsAsync()
        {
            var universeActors = await registry.GetAllRegisteredItemsAsync<ActorId>();
            return universeActors;
        }

        /// <summary>
        /// Get a single universe actor by device id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<KeyValuePair<string, ActorId>> GetRegisteredUniverseActorAsync(string key)
        {
            var universeActor = await registry.GetRegisteredItemAsync<ActorId>(key);
            return universeActor;
        }

        /// <summary>
        /// Register a new universe actor id map
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public async Task<bool> RegisterUniverseActorAsync(string id, ActorId actorId)
        {
            var success = await registry.RegisterAsync<ActorId>(id, actorId);
            return success;
        }

        /// <summary>
        /// Create service instance listeners
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }
    }
}
