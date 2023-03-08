using AzureCosmosDbManager.Attributes;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Context
{
    public class DatabaseHelper
    {
        private string _account;
        private string _key;
        private string _database;

        public DatabaseHelper(string account, string key, string database)
        {
            _account = account;
            _key = key;
            _database = database;
        }

        public async Task<Container> GetContainerAsync(Type itemType, bool forceContainerSync = false)
        {
            try
            {
                var cosmoClient = new CosmosClient(_account, _key);
                Database database = cosmoClient.GetDatabase(_database);

                var containerName = GetContainerName(itemType);
                var partitionKeyPath = GetPartitionKeyPath(itemType);
                var container = await database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath);

                return container;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetContainerName(Type itemType)
        {
            Type attributeType = typeof(ContainerNameAttribute);

            Attribute attribute = Attribute.GetCustomAttribute(itemType, attributeType);

            return attribute is ContainerNameAttribute containerAttribute
                ? containerAttribute.Name
                : itemType.Name;
        }

        private string GetPartitionKeyPath(Type itemType)
        {
            Type attributeType = typeof(PartitionKeyPathAttribute);

            return Attribute.GetCustomAttribute(
                itemType, attributeType) is PartitionKeyPathAttribute partitionKeyPathAttribute
                ? partitionKeyPathAttribute.Path
                : "/id";
        }
    }
}
