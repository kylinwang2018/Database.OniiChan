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
        public static IServiceCollection UsePostgreSql<T>(
            this IServiceCollection serviceCollection) where T : class, IDbContextOptions
        {
            // register dbprovider in service collection
            serviceCollection.TryAddScoped<INpgsqlDbContext<T>, NpgsqlDbContext<T>>();

            // register sql factory for create connection, command and dataAdapter
            serviceCollection.TryAddScoped<INpgsqlConnectionFactory, NpgsqlConnectionFactory>();

            return serviceCollection;
        }

        /// <summary>
        /// <para>
        /// inject all project-related repository with [PostgreRepo]
        /// (<see cref="PostgreRepoAttribute"/>) attribute to <see cref="IServiceCollection"/>.
        /// </para>
        /// <para>
        /// Make sure your repository projects has been add to your main project and its assembly
        /// name must start as same as your main project's, otherwise it cannot find the repositories.
        /// </para>
        /// <para>
        /// For example: your main project named "ExampleService", and its repository project must be
        /// "ExampleService.Repo" or "ExampleService[anything]"
        /// </para>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblyNameStart"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterPostgreRepositories(
            this IServiceCollection serviceCollection, string? assemblyNameStart)
        {
            var allAssembly = AppAssembly.GetAll(assemblyNameStart);

            serviceCollection.RegisterServiceByAttribute(ServiceLifetime.Singleton, allAssembly);
            serviceCollection.RegisterServiceByAttribute(ServiceLifetime.Scoped, allAssembly);
            serviceCollection.RegisterServiceByAttribute(ServiceLifetime.Transient, allAssembly);

            return serviceCollection;
        }

        private static void RegisterServiceByAttribute(this IServiceCollection services, ServiceLifetime serviceLifetime, Assembly[] allAssembly)
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
