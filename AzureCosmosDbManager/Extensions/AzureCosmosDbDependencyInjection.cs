using AzureCosmosDbManager.Context;
using AzureCosmosDbManager.Interfaces;
using AzureCosmosDbManager.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace AzureCosmosDbManager.Extensions
{
    public static class AzureCosmosDbDependencyInjection
    {
        public static void CosmosDbManagerAdd(this IServiceCollection services, string account, string key, string database)
        {
            var azureCosmosDbContext = new DatabaseContext(account, key, database);
            services.AddScoped<IDatabaseContext>(x => azureCosmosDbContext);
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}
