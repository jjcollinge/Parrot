using Common.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;

namespace Common.Services
{
    public class ServiceProxyFactory : Interfaces.IServiceProxyFactory
    {
        public IUniverseActorRegistry CreateUniverseActorRegistryServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseActorRegistry>(serviceAddress);
        }

        public IUniverseBuilder CreateUniverseBuilderServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseBuilder>(serviceAddress);
        }

        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseRegistry>(serviceAddress);
        }

        public IUniverseTemplateBuilder CreateUniverseTemplateBuilderServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseTemplateBuilder>(serviceAddress);
        }

        public IUniverseTemplateLoader CreateUniverseTemplateLoaderServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseTemplateLoader>(serviceAddress);
        }
    }
}
