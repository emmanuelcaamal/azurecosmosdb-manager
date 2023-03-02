using System.Threading.Tasks;

namespace AzureCosmosDbManager.Interfaces
{
    public interface IBaseRepository<T> : IBaseReadPagingRepository<T>, IBaseReadRepository<T> where T : IItem
    {
        Task<T> Save(T item);

        Task<bool> Delete(string id);
    }
}
