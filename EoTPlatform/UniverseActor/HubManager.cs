using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActor
{
    public class HubManager
    {
        private string hubConnectionString;
        private static RegistryManager registryManager;
        private static Device device;

        public HubManager(string hubConnectionString)
        {
            this.hubConnectionString = hubConnectionString;
        }

        public async Task RegisterAsync(string deviceId)
        {
            registryManager = RegistryManager.CreateFromConnectionString(hubConnectionString);

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
                throw ex;
            }

            ActorEventSource.Current.Message("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }

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
    }
}
