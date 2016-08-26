using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mocks
{
    public class MockCloudConnector : ICloudConnector
    {
        public Task DeregisterAsync()
        {
            return Task.FromResult(true);
        }

        public Task<string> ReceiveMessageAsync()
        {
            return Task.FromResult("Hello World");
        }

        public Task RegisterAsync(string deviceId, string hostname, string policyName, string deviceKey)
        {
            return Task.FromResult(true);
        }

        public Task SendMessageAsync(string msg)
        {
            return Task.FromResult(true);
        }
    }
}
