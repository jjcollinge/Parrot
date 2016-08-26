using Common;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActor
{
    public class AzureConnector : ICloudConnector
    {
        private string hostname;
        private string policyName;
        private string deviceKey;
        private string connectionString;
        private RegistryManager registryManager;
        private Device device;
        private string deviceId;

        private MessageSender sender;
        private MessageReceiver receiver;

        public AzureConnector()
        {}

        /// <summary>
        /// Register device with Azure IoTHub
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="hostname"></param>
        /// <param name="policyName"></param>
        /// <param name="deviceKey"></param>
        /// <returns></returns>
        public async Task RegisterAsync(string deviceId, string hostname, string policyName, string deviceKey)
        {
            this.hostname = hostname;
            this.deviceId = deviceId;
            this.policyName = policyName;
            this.deviceKey = deviceKey;
            this.connectionString = $"HostName={hostname};SharedAccessKeyName={policyName};SharedAccessKey={deviceKey}";

            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            sender = new MessageSender(connectionString);
            receiver = new MessageReceiver(deviceId, hostname, policyName, deviceKey);

            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            catch (Exception ex)
            {
                //TODO: Handle exception
                // For some reason DeviceAlreadyExistsException is landing here
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            ActorEventSource.Current.Message("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }

        /// <summary>
        /// Deregister device from Azure IoTHub.
        /// </summary>
        /// <returns></returns>
        public async Task DeregisterAsync()
        {
            try
            {
                await registryManager?.RemoveDeviceAsync(device);
            }
            catch (DeviceNotFoundException)
            {
                //TODO: Handle ex
            }
        }
        
        public async Task SendMessageAsync(string msg)
        {
            await sender.SendMessageAsync(deviceId, Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// Receive a message from Azure IoTHub.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReceiveMessageAsync()
        {
            var msg = await receiver.ReceiveMessageAsync();

            string body = "";
            using (var reader = new StreamReader(msg.BodyStream))
            {
                body = await reader.ReadToEndAsync();
            }

            return body;
        }
    }
}
