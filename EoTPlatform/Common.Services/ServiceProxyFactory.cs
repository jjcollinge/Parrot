using Common.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;

namespace Common.Services
{
    public class ServiceProxyFactory : Interfaces.IServiceProxyFactory
    {
        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseRegistry>(serviceAddress);
        }
    }
}
