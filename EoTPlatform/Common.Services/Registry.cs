using Common.Interfaces;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Services
{
    public class Registry : IRegistry
    {
        protected string StorageKey { get; } = "RegistryStore";
        protected IReliableStateManager StateManager { get; }

        public Registry(StatefulServiceContext context, IReliableStateManager stateManager)
        {
            StateManager = stateManager;
        }

        public async Task<bool> RegisterAsync<T>(string key, T value)
        {
            var storage = await StateManager.GetOrAddAsync<IReliableDictionary<string, T>>(StorageKey);
            bool success = false;

            using (var tx = this.StateManager.CreateTransaction())
            {
                success = await storage.TryAddAsync(tx, key, value);
                await tx.CommitAsync();
            }

            return success;
        }

        public async Task<bool> DeregisterAsync<T>(string key)
        {
            var storage = await StateManager.GetOrAddAsync<IReliableDictionary<string, T>>(StorageKey);
            bool success = false;

            using (var tx = this.StateManager.CreateTransaction())
            {
                var res = await storage.TryRemoveAsync(tx, key);

                if (res.HasValue)
                    success = true;

                await tx.CommitAsync();
            }

            return success;
        }

        public async Task<IDictionary<string, T>> GetAllRegisteredItemsAsync<T>()
        {
            var storage = await StateManager.GetOrAddAsync<IReliableDictionary<string, T>>(StorageKey);

            var items = new Dictionary<string, T>();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await storage.CreateEnumerableAsync(tx);
                var enumerator = enumerable.GetAsyncEnumerator();
                var cancelToken = new CancellationToken();
                while (await enumerator.MoveNextAsync(cancelToken))
                {
                    items.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
            }

            return items;

        }

        public async Task<KeyValuePair<string, T>> GetRegisteredItemAsync<T>(string key)
        {
            var storage = await StateManager.GetOrAddAsync<IReliableDictionary<string, T>>(StorageKey);

            var item = new KeyValuePair<string, T>();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await storage.TryGetValueAsync(tx, key);

                if (result.HasValue)
                    item = new KeyValuePair<string, T>(key, result.Value);
            }

            return item;
        }

        public async Task ClearAllAsync<T>()
        {
            var storage = await StateManager.GetOrAddAsync<IReliableDictionary<string, T>>(StorageKey);
            using (var tx = this.StateManager.CreateTransaction())
            {
                await storage.ClearAsync();
                await tx.CommitAsync();
            }
        }
    }
}
