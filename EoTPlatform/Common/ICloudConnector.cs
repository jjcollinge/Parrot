using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ICloudConnector
    {
        Task RegisterAsync(string deviceId, string hostname, string policyName, string deviceKey);
        Task DeregisterAsync();
        Task SendMessageAsync(string msg);
        Task<string> ReceiveMessageAsync();
    }
}
