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
        private string deviceId;

        public MessageReceiver(string deviceId, string hostname, string policyName, string deviceKey)
        {
            this.deviceId = deviceId;
            this.client = DeviceClient.Create(hostname, new DeviceAuthenticationWithSharedAccessPolicyKey(deviceId, policyName, deviceKey));
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var message = await client.ReceiveAsync();
            ActorEventSource.Current.Message($"Recieved message {message}");
            await client.CompleteAsync(message);

            string body = "";
            using(var reader = new StreamReader(message.BodyStream))
            {
                body = await reader.ReadToEndAsync();
            }

            return body;
        }
    }
}
