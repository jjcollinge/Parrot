using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseScheduler
{
    public interface IEventStreamFileLoader
    {
        Queue<UniverseEvent> ReadEventStreamFromFile(string filePath);
    }
}
