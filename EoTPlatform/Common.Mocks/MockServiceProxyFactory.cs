using System;
using Common.Interfaces;
using UniverseRegistry.Mocks;

namespace Common.Mocks
{
    public class MockServiceProxyFactory : IServiceProxyFactory
    {
        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return new MockUniverseRegistry();
        }
    }
}
