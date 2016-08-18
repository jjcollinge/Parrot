using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseBuilder.Tests
{
    public class MockPlatformWrapper : IPlatformWrapper
    {
        public Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName)
        {
            return Task.FromResult(true);
        }

        public Task<ServiceContext> GetServiceContextAsync()
        {
            return null;
        }

        public Task<string> GetServiceContextApplicationNameAsync()
        {
            return Task.FromResult("mock");
        }
    }
}
