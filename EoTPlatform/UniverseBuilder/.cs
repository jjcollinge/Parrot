using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services.Interfaces;

namespace UniverseBuilder
{
    public class MockServiceProxyFactory : IServiceProxyFactory
    {
        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            return null;
        }
    }
}
