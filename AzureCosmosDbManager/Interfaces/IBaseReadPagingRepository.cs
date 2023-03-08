using AzureCosmosDbManager.Response;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Interfaces
{
    public interface IBaseReadPagingRepository<T> where T : IItem
    {
        Task<PagingResponse<TResult>> Get<TResult>(string queryString, int pageSize = 20, string continuationToken = null)
            where TResult : class;
    }
}
