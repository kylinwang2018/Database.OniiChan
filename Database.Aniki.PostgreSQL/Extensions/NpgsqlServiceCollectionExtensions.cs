using Database.Aniki.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Aniki
{
    /// <summary>
    ///     PostgreSql specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class NpgsqlServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the context to connect to a Postgre SQL database, must set up connection string before use it.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection UsePostgreSql(
            this IServiceCollection serviceCollection)
        {
            // register dbprovider in service collection
            serviceCollection.AddScoped<INpgsqlDbContext, NpgsqlDbContext>();

            // register sql factory for create connection, command and dataAdapter
            serviceCollection.AddScoped<INpgsqlConnectionFactory, NpgsqlConnectionFactory>();

            return serviceCollection;
        }
    }
}
