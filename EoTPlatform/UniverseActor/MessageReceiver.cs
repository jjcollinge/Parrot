using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActor
{
    public class MessageReceiver
    {
        private DeviceClient client;

        public MessageReceiver(string deviceId, string hostname, string policyName, string deviceKey)
        {
            this.client = DeviceClient.Create(hostname, new DeviceAuthenticationWithSharedAccessPolicyKey(deviceId, policyName, deviceKey));
        }

        /// <summary>
        /// Receive a message from the cloud and parse it into a string.
        /// </summary>
        /// <returns></returns>
        public async Task<Message> ReceiveMessageAsync()
        {
            var message = await client.ReceiveAsync();
            await client.CompleteAsync(message);

            return message;
        }
    }
}
