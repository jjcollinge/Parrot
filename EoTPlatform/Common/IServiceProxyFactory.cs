using System;

namespace Common.Interfaces
{
    //TODO: Consider using in built remoting service factory
    public interface IServiceProxyFactory
    {
        IUniverseRegistry CreateUniverseRegistryServiceProxy(Uri serviceAddress);
        IUniverseBuilder CreateUniverseBuilderServiceProxy(Uri serviceAddress);
        IUniverseTemplateLoader CreateUniverseTemplateLoaderServiceProxy(Uri serviceAddress);
        IUniverseTemplateBuilder CreateUniverseTemplateBuilderServiceProxy(Uri serviceAddress);
    }
}
