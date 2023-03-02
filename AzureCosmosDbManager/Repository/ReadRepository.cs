using AzureCosmosDbManager.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Repository
{
    public class ReadRepository<T> : ReadPagingRepository<T>, IBaseReadRepository<T> where T : IItem
    {
        private readonly IDatabaseContext _dbContext;

        public ReadRepository(IDatabaseContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var container = await _dbContext.GetContainerAsync<T>();
                using (var feedIterator = container.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: true,
                    requestOptions: new QueryRequestOptions { MaxItemCount = 1 })
                    .Where(predicate).ToFeedIterator())
                {

                    while (feedIterator.HasMoreResults)
                    {
                        var response = await feedIterator.ReadNextAsync();
                        if (response.Any())
                            return response.FirstOrDefault();
                    }
                }

                return default(T);
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var container = await _dbContext.GetContainerAsync<T>();
                using (var feedIterator = container.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: true)
                    .Where(predicate).ToFeedIterator())
                {
                    while (feedIterator.HasMoreResults)
                    {
                        var response = await feedIterator.ReadNextAsync();
                        if (response.Any())
                            return response.ToList();
                    }

                    return new List<T>();
                }
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }

        public async Task<List<T>> Get(string queryString)
        {
            try
            {
                var container = await _dbContext.GetContainerAsync<T>();
                var feedIterator = container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();
                    if (response.Any())
                        return response.ToList();
                }

                return new List<T>();
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }

        public async Task<List<TResult>> Get<TResult>(string queryString)
            where TResult : class
        {
            try
            {
                var result = new List<TResult>();
                var container = await _dbContext.GetContainerAsync<T>();
                var feedIterator = container.GetItemQueryIterator<TResult>(new QueryDefinition(queryString));
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();
                    if (response.Any())
                        result.AddRange(response.ToList());
                }

                return result;
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }
    }
}
