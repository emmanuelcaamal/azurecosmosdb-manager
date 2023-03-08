using AzureCosmosDbManager.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Context
{
    public sealed class DatabaseContext : DatabaseHelper, IDatabaseContext
    {
        private Dictionary<(Type type, string name), object> _containers;

        public DatabaseContext(string account, string key, string database)
            : base(account, key, database)
        {
        }

        public async Task<Container> GetContainerAsync<TItem>(bool forceContainerSync = false) where TItem : IItem
        {
            _containers = _containers ?? new Dictionary<(Type type, string name), object>();
            if (_containers.TryGetValue((typeof(TItem), typeof(TItem).FullName), out var containerResult))
                return (Container)containerResult;
            else
            {
                var container = await GetContainerAsync(typeof(TItem), forceContainerSync);
                _containers.Add((typeof(TItem), typeof(TItem).FullName), container);
                return container;
            }
        }
    }
}
