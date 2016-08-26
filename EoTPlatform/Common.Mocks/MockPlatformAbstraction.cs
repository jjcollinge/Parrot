using Common.Interfaces;
using System;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using UniverseActor.Interfaces;
using UniverseActor.Mocks;

namespace Common.Mocks
{
    public class MockPlatformAbstraction : IPlatformAbstraction
    {
        public Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName, ServiceContextTypes type)
        {
            return Task.FromResult(true);
        }

        public Task<ServiceContext> GetServiceContextAsync()
        {
            return null;
        }

        public Task<string> GetServiceContextApplicationNameAsync()
        {
            return Task.FromResult("fabric:/mock/mock");
        }

        public async Task<IUniverseActor> CreateUniverseActorProxyAsync(ActorId actorId, Uri serviceAddress)
        {
            return new MockUniverseActor();
        }

        public Task<ActorId> GetActorIdAsync(string actorIdAsString)
        {
            return Task.FromResult(ActorId.CreateRandom());
        }
    }
}
