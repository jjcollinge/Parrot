using System;
using System.Fabric;
using System.Threading.Tasks;

namespace Common.Interfaces
{
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
