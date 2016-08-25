using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Models;
using Common.Interfaces;

namespace UniverseFactory
{
    public sealed class UniverseFactory : StatelessService, IUniverseFactory
    {
        //TODO: Consider packaging into NuGet
        private IServiceProxyFactory serviceFactory;

        public UniverseFactory(StatelessServiceContext context, IServiceProxyFactory serviceFactory)
            : base(context)
        {
            this.serviceFactory = serviceFactory;
        }

        public async Task<UniverseDefinition> CreateUniverseAsync(UniverseSpecification specification)
        {
            // Assume universe template file exists
            // Could use specification id as partition id if required

            // Load universe template file
            var templateLoaderAddress = new Uri("fabric:/EoTPlatform/UniverseTemplateLoader");  
            var templateLoader = serviceFactory.CreateUniverseTemplateLoaderServiceProxy(templateLoaderAddress);
            var filePath = specification.UniverseTemplateFilePath;
            var universeTemplate = await templateLoader.LoadUniversalTemplateFromFileAsync(filePath);

            // Build universe
            var universeBuilderAddress = new Uri("fabric:/EoTPlatform/UniverseBuilder");
            var universeBuilder = serviceFactory.CreateUniverseBuilderServiceProxy(universeBuilderAddress);
            var universeDescriptor = await universeBuilder.BuildUniverseAsync(specification.UniverseDataSourceFilePath, universeTemplate);

            // Register the universe
            var universeRegistryAddress = new Uri("fabric:/EoTPlatform/UniverseRegistry");
            var universeRegistry = serviceFactory.CreateUniverseRegistryServiceProxy(universeRegistryAddress);
            await universeRegistry.RegisterUniverseAsync(universeDescriptor);

            // Return descriptor
            return universeDescriptor;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context))
            };
        }

    }
}
