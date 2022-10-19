using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Database.Aniki
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class SqlServerServiceCollectionExtensions
    {
        public static IServiceCollection UseSqlServer(
            this IServiceCollection serviceCollection)
        {
            // register dbprovider in service collection
            return serviceCollection.AddScoped<ISqlServerDbContext, SqlServerDbContext>();
        }
    }
}
