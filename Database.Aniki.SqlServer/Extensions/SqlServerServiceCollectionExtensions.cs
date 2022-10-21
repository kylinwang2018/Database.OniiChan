using Database.Aniki.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Database.Aniki
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the context to connect to a Microsoft SQL Server database, must set up connection string before use it.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection UseSqlServer(
            this IServiceCollection serviceCollection)
        {
            // register dbprovider in service collection
            serviceCollection.TryAddScoped<ISqlServerDbContext, SqlServerDbContext>();

            // register sql factory for create connection, command and dataAdapter
            serviceCollection.TryAddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

            return serviceCollection;
        }
    }
}
