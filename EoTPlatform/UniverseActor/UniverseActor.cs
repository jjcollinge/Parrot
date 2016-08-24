using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using UniverseActor.Interfaces;
using Common.Models;
using System.Text;

namespace UniverseActor
{
    [StatePersistence(StatePersistence.Persisted)]
    public class UniverseActor : Actor, IUniverseActor, IRemindable
    {
        // Communications
        private MessageSender sender;
        private MessageReceiver receiver;
        private HubManager hub;

        private static string deviceId;
        private static bool isConnected = false;

        // Template
        private ActorTemplate template { get; set; }

        public UniverseActor()
        {
            sender = new MessageSender(deviceId, "HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
            receiver = new MessageReceiver(deviceId, "eotiothub.azure-devices.net", "iothubowner", "Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
            hub = new HubManager("HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
        }
        
        public async Task SendMessageAsync(string msg)
        {
            if (isConnected)
            {
                await sender.SendMessageAsync(Encoding.UTF8.GetBytes(msg));
            }
        }

        public async Task ReceiveMessageAsync()
        {
            if(isConnected)
            {
                var message = await receiver.ReceiveMessageAsync();

                // Assumes message is just command name
                if(template.Commands.Contains(message))
                {
                    // Invoke method via reflection
                }
            }
        }

        public Task EnableAsync()
        {
            throw new NotImplementedException();
        }

        public Task DisableAsync()
        {
            throw new NotImplementedException();
        }

        public Task RestartAsync()
        {
            throw new NotImplementedException();
        }

        private async Task RegisterReminder()
        {

        }
    
        public async Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            
        }

        public Task<bool> IsConnectedAsync()
        {
            return Task.FromResult(isConnected);
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected async override Task OnActivateAsync()
        {
            deviceId = this.Id.GetStringId();
            await hub.RegisterAsync(deviceId);
        }

        protected async override Task OnDeactivateAsync()
        {
            await hub.DeregisterAsync();
        }

        public Task SetTemplate(ActorTemplate template)
        {
            this.template = template;
            return Task.FromResult(true);
        }
    }
}
