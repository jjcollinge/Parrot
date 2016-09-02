using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IRegistry
    {
        Task<bool> RegisterAsync<T>(string key, T value);
        Task<bool> DeregisterAsync<T>(string key);
        Task<IDictionary<string, T>> GetAllRegisteredItemsAsync<T>();
        Task<KeyValuePair<string, T>> GetRegisteredItemAsync<T>(string key);
        Task ClearAllAsync<T>();
    }
}
