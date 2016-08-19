﻿using Common.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;

namespace Common.Services
{
    public class ServiceProxyFactory : Interfaces.IServiceProxyFactory
    {
        public IUniverseActorRegistry CreateUniverseActorRegistryServiceProxy(Uri serviceAddress)
        {
            // No partition key needed as currently uses singleton partition scheme
            return ServiceProxy.Create<IUniverseActorRegistry>(serviceAddress);
        }

        public IUniverseBuilder CreateUniverseBuilderServiceProxy(Uri serviceAddress)
        {
            return ServiceProxy.Create<IUniverseBuilder>(serviceAddress);
        }

        public IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress)
        {
            // Uses int64 partition scheme so requires key
            return ServiceProxy.Create<IUniverseRegistry>(serviceAddress, new ServicePartitionKey(1L));
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
