using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    //TODO: Need to consider platform abstraction mechanism

    public interface IPlatformAbstraction
    {
        Task<string> GetServiceContextApplicationNameAsync();
        Task<ServiceContext> GetServiceContextAsync();
        Task BuildServiceAsync(string applicationName, Uri serviceAddress, string serviceTypeName, ServiceContextTypes type);
    }

    public enum ServiceContextTypes
    {
        Stateful,
        Stateless
    }
}
