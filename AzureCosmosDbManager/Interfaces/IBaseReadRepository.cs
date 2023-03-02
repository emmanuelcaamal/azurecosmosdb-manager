using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.Interfaces
{
    public interface IBaseReadRepository<T> : IBaseReadPagingRepository<T> where T : IItem
    {
        Task<T> Find(Expression<Func<T, bool>> predicate);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate);
        Task<List<T>> Get(string queryString);
        Task<List<TResult>> Get<TResult>(string queryString) where TResult : class;
    }
}
