using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace UniverseBuilder
{
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseRegistry>(serviceAddress);
        }
    }
}
