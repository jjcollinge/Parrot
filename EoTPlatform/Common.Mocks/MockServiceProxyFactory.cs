using System;
using Common.Interfaces;
using UniverseRegistry.Mocks;
using UniverseBuilder.Mocks;
using UniverseTemplateBuilder.Mocks;
using UniverseTemplateLoader.Mocks;
using UniverseActorRegistry.Mock;

namespace Common.Mocks
{
    public class MockServiceProxyFactory : IServiceProxyFactory
    {
        public IUniverseActorRegistry CreateUniverseActorRegistryServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseActorRegistry();
        }

        public IUniverseBuilder CreateUniverseBuilderServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseBuilder();
        }

        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseRegistry();
        }

        public IUniverseTemplateBuilder CreateUniverseTemplateBuilderServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseTemplateBuilder();
        }

        public IUniverseTemplateLoader CreateUniverseTemplateLoaderServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseTemplateLoader();
        }
    }
}
