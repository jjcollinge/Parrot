using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using UniverseActor.Interfaces;

namespace Common.Interfaces
{
    //TODO: Need to consider platform abstraction mechanism

    public interface IPlatformAbstraction
    {
        Task<string> GetServiceContextApplicationNameAsync();
        Task<ServiceContext> GetServiceContextAsync();
        Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName, ServiceContextTypes type);
        Task<IUniverseActor> CreateUniverseActorProxyAsync(ActorId actorId, Uri serviceAddress);
        Task<ActorId> GetActorIdAsync(string actorIdAsString);
    }

    public enum ServiceContextTypes
    {
        Stateful,
        Stateless
    }
}
