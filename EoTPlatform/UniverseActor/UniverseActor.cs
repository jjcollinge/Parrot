using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using UniverseActor.Interfaces;
using Common.Models;
using System.Text;
using System.Collections.Generic;

namespace UniverseActor
{
    [StatePersistence(StatePersistence.Persisted)]
    public class UniverseActor : Actor, IUniverseActor
    {
        private MessageSender sender;
        private MessageReceiver receiver;
        private HubManager hub;
        private CommandService commands;
        private IActorTimer pushTimer;

        public string deviceId { get; set; }

        // Template
        public ActorTemplate template { get; private set; }

        public UniverseActor(string id)
        {
            deviceId = id;
            sender = new MessageSender(deviceId, "HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
            receiver = new MessageReceiver(deviceId, "eotiothub.azure-devices.net", "iothubowner", "Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
        }

        public async Task SendMessageAsync(string msg)
        {
            await sender.SendMessageAsync(Encoding.UTF8.GetBytes(msg));
        }

        public async Task ReceiveMessageAsync()
        {
            var message = await receiver.ReceiveMessageAsync();

            // Assumes message is just command name
            await commands.InvokeAsync(message);
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

        private async Task PushStatusAsync(object state)
        {
            await sender.SendMessageAsync(Encoding.UTF8.GetBytes("status:good"));
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected async override Task OnActivateAsync()
        {
            if (deviceId == null)
            {
                hub = new HubManager("HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
                deviceId = this.Id.GetStringId();
                commands = new CommandService(new List<string>(this.template.Commands));
                pushTimer = this.RegisterTimer(
                    this.PushStatusAsync,
                    null,
                    TimeSpan.FromMilliseconds(5000),
                    TimeSpan.FromMilliseconds(1000));
            }

            await hub?.RegisterAsync(deviceId);
        }

        protected async override Task OnDeactivateAsync()
        {
            await hub?.DeregisterAsync();
            hub = null;
        }

        public Task SetTemplate(ActorTemplate template)
        {
            this.template = template;
            return Task.FromResult(true);
        }
    }
}
