using Common.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseBuilder
{
    public interface IServiceProxyFactory
    {
        IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress);
    }
}
