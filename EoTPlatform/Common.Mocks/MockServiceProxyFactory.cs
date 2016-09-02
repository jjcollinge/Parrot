using System;
using Common.Interfaces;
using UniverseRegistry.Mocks;
using UniverseBuilder.Mocks;
using UniverseTemplateLoader.Mocks;
using UniverseActorRegistry.Mocks;
using UniverseScheduler.Mocks;
using UniverseFactory.Mocks;

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

        public IUniverseFactory CreateUniverseFactory(Uri serviceAddress)
        {
            return new MockUniverseFactory();
        }

        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseRegistry();
        }

        public IUniverseScheduler CreateUniverseScheduler(Uri serviceAddress)
        {
            return new MockUniverseScheduler();
        }

        public IUniverseTemplateLoader CreateUniverseTemplateLoaderServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseTemplateLoader();
        }
    }
}
