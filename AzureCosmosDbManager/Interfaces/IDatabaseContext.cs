using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Interfaces
{
    public interface IDatabaseContext
    {
        Task<Container> GetContainerAsync<TItem>(bool forceContainerSync = false) where TItem : IItem;
    }
}
