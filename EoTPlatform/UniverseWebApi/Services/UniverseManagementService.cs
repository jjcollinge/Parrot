using Common;
using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseWebApi.Services
{
    public class UniverseManagementService : IUniverseManagementService
    {
        private IServiceProxyFactory proxyFactory;

        public UniverseManagementService(IServiceProxyFactory proxyFactory)
        {
            this.proxyFactory = proxyFactory;
        }

        public async Task CreateUniverseAsync(string universeTemplateFilePath)
        {
            var spec = new UniverseSpecification();
            spec.Id = Guid.NewGuid().ToString();
            spec.UniverseTemplateFilePath = universeTemplateFilePath;

            var universeFactory = proxyFactory.CreateUniverseFactory(new Uri("fabric:/EoTPlatform/UniverseFactory"));
            await universeFactory.CreateUniverseAsync(spec);
        }

        public async Task DeleteAllUniversesAsync()
        {
            // Delete all support services
            var registry = proxyFactory.CreateUniverseRegistryServiceProxy(new Uri("fabric:/EoTPlatform/UniverseRegistry"));
            var univereses = await registry.GetUniversesAsync();
            foreach(var universe in univereses.Values)
            {
                foreach(var endpoint in universe.ServiceEndpoints)
                {
                    // Delete service
                }
            }
        }

        public async Task DeleteUniverseAsync(string universeId)
        {
            // Delete any support services
            var registry = proxyFactory.CreateUniverseRegistryServiceProxy(new Uri("fabric:/EoTPlatform/UniverseRegistry"));
            var universe = await registry.GetUniverseAsync(universeId);
            foreach(var endpoint in universe.ServiceEndpoints)
            {
                // Delete service
            }

            // Remove from universe registry
            await registry.DeregisterUniverseAsync(universeId);
        }

        public async Task<UniverseDefinition> GetUniverseDescriptorAsync(string universeId)
        {
            var registry = proxyFactory.CreateUniverseRegistryServiceProxy(new Uri("fabric:/EoTPlatform/UniverseRegistry"));
            var universe = await registry.GetUniverseAsync(universeId);
            return universe;
        }

        public async Task<Dictionary<string, UniverseDefinition>> GetUniverseDescriptorsAsync()
        {
            var registry = proxyFactory.CreateUniverseRegistryServiceProxy(new Uri("fabric:/EoTPlatform/UniverseRegistry"));
            var universes = await registry.GetUniversesAsync();
            return universes;
        }

        public Task PauseUniverseAsync(string universeId)
        {
            // Stop streaming data
            throw new NotImplementedException();
        }
    }
}
