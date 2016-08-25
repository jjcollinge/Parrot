using Common.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Fabric;
using System.Fabric.Description;
using System.Threading.Tasks;
using UniverseActor.Interfaces;

namespace Common.Services
{
    //TODO: Consider packaging into NuGet

    public class PlatformAbstraction : IPlatformAbstraction
    {
        ServiceContext context;

        public PlatformAbstraction(ServiceContext context)
        {
            this.context = context;
        }

        public async Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName, ServiceContextTypes type)
        {
            using (var client = new FabricClient())
            {
                if (type == ServiceContextTypes.Stateful)
                {
                    var serviceDescriptor = new StatefulServiceDescription()
                    {
                        ApplicationName = new Uri(applicationName),
                        HasPersistedState = true,
                        ServiceName = serviceAddress,
                        ServiceTypeName = serviceTypeName,
                        PartitionSchemeDescription = new SingletonPartitionSchemeDescription() //TODO: Parameterise and use better partition scheme
                  
                    };
                    await client.ServiceManager.CreateServiceAsync(serviceDescriptor);
                }
                else
                {
                    var serviceDescriptor = new StatelessServiceDescription()
                    {
                        ApplicationName = new Uri(applicationName),
                        InstanceCount = -1,
                        ServiceName = serviceAddress,
                        ServiceTypeName = serviceTypeName,
                        PartitionSchemeDescription = new SingletonPartitionSchemeDescription() //TODO: Parameterise and use better partition scheme
                    };
                    await client.ServiceManager.CreateServiceAsync(serviceDescriptor);
                }
            }
        }

        public Task<ServiceContext> GetServiceContextAsync()
        {
            return Task.FromResult(context);
        }

        public Task<string> GetServiceContextApplicationNameAsync()
        {
            return Task.FromResult(context.CodePackageActivationContext.ApplicationName);
        }

        public Task<IUniverseActor> CreateUniverseActorProxyAsync(ActorId actorId, Uri serviceAddress)
        {
            return Task.FromResult(ActorProxy.Create<IUniverseActor>(actorId, serviceAddress));
        }

        public Task<ActorId> GetActorIdAsync(string actorIdAsString)
        {
            return Task.FromResult(ActorId.CreateRandom());
        }
    }
}
