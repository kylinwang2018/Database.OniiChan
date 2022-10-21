using Database.Aniki.PostgreSQL;
using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

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
            serviceCollection.TryAddScoped<INpgsqlDbContext, NpgsqlDbContext>();

            // register sql factory for create connection, command and dataAdapter
            serviceCollection.TryAddScoped<INpgsqlConnectionFactory, NpgsqlConnectionFactory>();

            return serviceCollection;
        }

        public static IServiceCollection RegisterPostgreRepositories(
            this IServiceCollection serviceCollection)
        {
            var allAssembly = AppAssembly.GetAll();

            serviceCollection.RegisterServiceByAttribute(ServiceLifetime.Singleton, allAssembly);
            serviceCollection.RegisterServiceByAttribute(ServiceLifetime.Scoped, allAssembly);
            serviceCollection.RegisterServiceByAttribute(ServiceLifetime.Transient, allAssembly);

            return serviceCollection;
        }

        private static void RegisterServiceByAttribute(this IServiceCollection services, ServiceLifetime serviceLifetime, List<Assembly> allAssembly)
        {
            List<Type> types = allAssembly
                .SelectMany(t => 
                t.GetTypes())
                    .Where(t => 
                        t.GetCustomAttributes(typeof(PostgreRepoAttribute), false).Length > 0 && 
                            t.GetCustomAttribute<PostgreRepoAttribute>()?.Lifetime == serviceLifetime && 
                            t.IsClass && !t.IsAbstract).ToList();
            foreach (var type in types)
            {
                Type? typeInterface = type.GetInterfaces().FirstOrDefault();
                if (typeInterface != null)
                {
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton: 
                            services.TryAddSingleton(typeInterface, type); 
                            break;
                        case ServiceLifetime.Scoped: 
                            services.TryAddScoped(typeInterface, type); 
                            break;
                        case ServiceLifetime.Transient: 
                            services.TryAddTransient(typeInterface, type); 
                            break;
                    }
                }
            }
        }
    }
}
