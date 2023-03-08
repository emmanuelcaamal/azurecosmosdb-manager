using AzureCosmosDbManager.Interfaces;
using AzureCosmosDbManager.Response;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Repository
{
    public class ReadPagingRepository<T> : IBaseReadPagingRepository<T> where T : IItem
    {
        private readonly IDatabaseContext _dbContext;

        public ReadPagingRepository(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResponse<TResult>> Get<TResult>(string queryString, int pageSize = 20, string continuationToken = null)
            where TResult : class
        {
            try
            {
                if (string.IsNullOrEmpty(continuationToken))
                    continuationToken = null;
                else
                    continuationToken = DecodeContinuationToken(continuationToken);

                var result = new List<TResult>();
                int total = 0;
                var container = await _dbContext.GetContainerAsync<T>();
                var resultTotal = container.GetItemQueryIterator<dynamic>(new QueryDefinition("SELECT count(c.id) as Total FROM c"));
                var resultIterator = container.GetItemQueryIterator<TResult>(new QueryDefinition(queryString),
                requestOptions: new QueryRequestOptions { MaxItemCount = pageSize }, continuationToken: continuationToken);

                while (resultTotal.HasMoreResults)
                {
                    var responseTotal = await resultTotal.ReadNextAsync();
                    if (responseTotal.Any())
                    {
                        total = responseTotal.FirstOrDefault().Total;
                    }
                }

                while (resultIterator.HasMoreResults)
                {
                    var response = await resultIterator.ReadNextAsync();
                    if (response.Any())
                    {
                        result.AddRange(response.ToList());
                        continuationToken = EncodeContinuationToken(response.ContinuationToken);
                        break;
                    }
                } 

                return new PagingResponse<TResult>(pageSize, total, result, continuationToken);
            }
            catch (CosmosException cx)
            {
                throw;
            }
        }

        private static string EncodeContinuationToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var textBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return System.Convert.ToBase64String(textBytes);
        }
        private static string DecodeContinuationToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var base64Bytes = System.Convert.FromBase64String(token);
            return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }
    }
}
