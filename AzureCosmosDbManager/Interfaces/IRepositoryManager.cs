namespace AzureCosmosDbManager.Interfaces
{
    public interface IRepositoryManager
    {
        IBaseRepository<T> GetRepository<T>() where T : IItem;
    }
}
