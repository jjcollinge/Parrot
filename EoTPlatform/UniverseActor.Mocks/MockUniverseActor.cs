using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverseActor.Interfaces;

namespace UniverseActor.Mocks
{
    public class MockUniverseActor : IUniverseActor
    {
        public Task DisableAsync()
        {
            return Task.FromResult(true);
        }

        public Task EnableAsync()
        {
            return Task.FromResult(true);
        }

        public Task ProcessEventAsync(UniverseEvent evt)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveMessageAsync()
        {
            return Task.FromResult(true);
        }

        public Task RestartAsync()
        {
            return Task.FromResult(true);
        }

        public Task SendMessageAsync(string msg)
        {
            return Task.FromResult(true);
        }

        public Task SetupAsync(ActorTemplate template)
        {
            return Task.FromResult(true);
        }
    }
}
