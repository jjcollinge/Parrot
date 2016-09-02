using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Models;
using Common.Interfaces;
using Microsoft.ServiceFabric.Data;
using Common.Services;

namespace UniverseRegistry
{
    /// <summary>
    /// Stores entries for each currently created universe and their definitions
    /// </summary>
    public sealed class UniverseRegistry : StatefulService, IUniverseRegistry
    {
        private IRegistry registry;

        public UniverseRegistry(StatefulServiceContext context, IReliableStateManager stateManager)
            : base(context, stateManager as IReliableStateManagerReplica)
        {
            this.registry = new Registry(stateManager);
        }

        /// <summary>
        /// Return metadata for all registered universes
        /// </summary>
        /// <returns></returns>
        public async Task<IDictionary<string, UniverseDefinition>> GetAllUniverseAsync()
        {
            var universes = await registry.GetAllRegisteredItemsAsync<UniverseDefinition>();
            return universes;
        }

        /// <summary>
        /// Register a new universe.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public async Task<bool> RegisterUniverseAsync(string id, UniverseDefinition definition)
        {
            var success = await registry.RegisterAsync<UniverseDefinition>(id, definition);
            return success;
        }

        /// <summary>
        /// Deregister an existing universe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeregisterUniverseAsync(string id)
        {
            var success = await registry.DeregisterAsync<UniverseDefinition>(id);
            return success;
        }

        /// <summary>
        /// Get a specific universe's metadata.
        /// </summary>
        /// <param name="universeId"></param>
        /// <returns></returns>
        public async Task<KeyValuePair<string, UniverseDefinition>> GetUniverseAsync(string universeId)
        {
            var definition = await registry.GetRegisteredItemAsync<UniverseDefinition>(universeId);
            return definition;
        }

        /// <summary>
        /// Create service replicate listener endpoints.
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
