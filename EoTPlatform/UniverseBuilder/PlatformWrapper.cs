using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseBuilder
{
    public class PlatformWrapper : IPlatformWrapper
    {
        ServiceContext context;

        public PlatformWrapper(ServiceContext context)
        {
            this.context = context;
        }

        public async Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName)
        {
            using (var client = new FabricClient())
            {
                var serviceDescriptor = new StatefulServiceDescription()
                {
                    ApplicationName = new Uri(applicationName),
                    HasPersistedState = true,
                    ServiceName = serviceAddress,
                    ServiceTypeName = serviceTypeName
                };

                await client.ServiceManager.CreateServiceAsync(serviceDescriptor);
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
    }
}
