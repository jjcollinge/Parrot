using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using UniverseActor.Interfaces;
using Common.Models;
using System.Text;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Actors;

namespace UniverseActor
{
    [StatePersistence(StatePersistence.Persisted)]
    public class UniverseActor : Actor, IUniverseActor
    {
        // Private
        private MessageSender sender;
        private MessageReceiver receiver;
        private HubManager hub;
        private CommandService commands;

        // Public
        public ActorTemplate template { get; private set; }

        public UniverseActor()
        { }

        /// <summary>
        /// This method is called to send a message to the cloud hub.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string msg)
        {
            await sender.SendMessageAsync(Encoding.UTF8.GetBytes(msg));
            ActorEventSource.Current.ActorMessage(this, $"Sent '{msg}' to the cloud.");
        }

        /// <summary>
        /// This method is called when the actor recieves a message.
        /// </summary>
        /// <returns></returns>
        public async Task ReceiveMessageAsync()
        {
            var msg = await receiver.ReceiveMessageAsync();
            ActorEventSource.Current.ActorMessage(this, $"Recieved '{msg}' from the cloud.");

            // Assumes message is just command name
            await commands.InvokeAsync(msg);
        }

        /// <summary>
        /// Start the actor timer to push events to the cloud.
        /// </summary>
        /// <returns></returns>
        public Task EnableAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop this actor pushing events to the cloud.
        /// </summary>
        /// <returns></returns>
        public Task DisableAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Restart or reset everything to initial state.
        /// </summary>
        /// <returns></returns>
        public Task RestartAsync()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected async override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, $"Activating actor with Id: '{Id}'");

            // TODO: Consider using timer for periodic event streaming.
        }

        /// <summary>
        /// This methopd is called whenever an actor is deactivated.
        /// </summary>
        /// <returns></returns>
        protected async override Task OnDeactivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, $"Deactivating actor with Id: '{Id}'");

            // Deregister this actor from the Hub Manager's cloud connection
            await hub?.DeregisterAsync();
            hub = null;
        }

        /// <summary>
        /// This method is called to bootstrap any post creation intialisation that may be required.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public async Task SetupAsync(ActorTemplate template)
        {
            ActorEventSource.Current.ActorMessage(this, $"Setting up actor with Id: '{Id}'");

            // Store the template which describes this actors profile
            this.template = template;
            commands = new CommandService(new List<string>(this.template.Commands));

            // Initialise communication services
            var cloudId = template.Id;
            sender = new MessageSender(cloudId, "HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
            receiver = new MessageReceiver(cloudId, "eotiothub.azure-devices.net", "iothubowner", "Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");

            // Create new Hub Manager and register this actor using the external template id with the cloud gateway.
            hub = new HubManager("HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=");
            await hub.RegisterAsync(cloudId);
        }
    }
}
