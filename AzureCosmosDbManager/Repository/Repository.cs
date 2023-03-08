using AzureCosmosDbManager.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Repository
{
    public class Repository<T> : ReadRepository<T>,  IBaseRepository<T> where T : IItem
    {
        private readonly IDatabaseContext _dbContext;

        public Repository(IDatabaseContext dbContext) :
            base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var container = await _dbContext.GetContainerAsync<T>();
                var result = await container.DeleteItemAsync<T>(id, new PartitionKey(id));
                return true;
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }

        public async Task<T> Save(T item)
        {
            try
            {
                var container = await _dbContext.GetContainerAsync<T>();
                var result = await container.UpsertItemAsync(item, new PartitionKey(item.Id));
                
                return result.Resource;
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }
    }
}
