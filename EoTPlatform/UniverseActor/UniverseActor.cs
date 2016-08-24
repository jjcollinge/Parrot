using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using UniverseActor.Interfaces;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace UniverseActor
{
    [StatePersistence(StatePersistence.Persisted)]
    public class UniverseActor : Actor, IUniverseActor
    {
        private const string iotConnectionString = "HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=";
        private static RegistryManager registryManager;
        private static Device device;
        private static string deviceId = Guid.NewGuid().ToString();

        public UniverseActor()
        { }

        public Task DisableAsync()
        {
            throw new NotImplementedException();
        }

        public Task DispatchMessageAsync()
        {
            throw new NotImplementedException();
        }

        public Task EnableAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReceivedMessageAsync()
        {
            throw new NotImplementedException();
        }

        public Task RestartAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsConnectedAsync()
        {
            var connected = device != null ? true : false;
            return Task.FromResult(connected);
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected async override Task OnActivateAsync()
        {
            if(device == null)
            {
                await RegisterDeviceWithIoTHubAsync();
            }
        }

        private async Task RegisterDeviceWithIoTHubAsync()
        {
            registryManager = RegistryManager.CreateFromConnectionString(iotConnectionString);

            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            catch(Exception ex)
            {
                //TODO: Handle exception
                throw ex;
            }

            ActorEventSource.Current.Message("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
