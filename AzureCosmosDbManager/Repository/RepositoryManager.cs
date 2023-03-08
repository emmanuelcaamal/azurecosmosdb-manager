using AzureCosmosDbManager.Interfaces;
using System;
using System.Collections.Generic;

namespace AzureCosmosDbManager.Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly IDatabaseContext _dbContext;
        private Dictionary<(Type type, string name), object> _repositories;

        public RepositoryManager(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBaseRepository<T> GetRepository<T>() where T : IItem
        {
            return (IBaseRepository<T>)GetGenericRepository(typeof(T), new Repository<T>(_dbContext));
        }

        internal object GetGenericRepository(Type type, object repo)
        {
            _repositories = _repositories ?? new Dictionary<(Type type, string name), object>();

            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var respository))
                return respository;

            _repositories.Add((type, repo.GetType().FullName), repo);
            return repo;
        }
    }
}
