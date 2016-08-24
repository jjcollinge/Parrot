using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Common.Models;

namespace UniverseActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IUniverseActor : IActor
    {
        Task RestartAsync();
        Task DisableAsync();
        Task EnableAsync();
        Task ReceiveMessageAsync();
        Task SendMessageAsync(string msg);
        //TODO: Needs DI
        Task SetTemplate(ActorTemplate template);
    }
}
