using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActor
{
    public class CommandService
    {
        List<string> commands;

        public CommandService(List<string> commands)
        {
            this.commands = commands;
        }

        public async Task InvokeAsync(string command)
        {
            if(command.Contains(command))
            {
                // Use reflection to invoke the command
            }
        }
    }
}
