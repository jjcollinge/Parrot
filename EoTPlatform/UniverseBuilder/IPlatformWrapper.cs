using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseBuilder
{
    public interface IPlatformWrapper
    {
        Task<string> GetServiceContextApplicationNameAsync();
        Task<ServiceContext> GetServiceContextAsync();
        Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName);
    }
}
