using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
