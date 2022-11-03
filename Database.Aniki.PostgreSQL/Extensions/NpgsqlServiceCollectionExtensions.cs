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
        ///     Configures the context to connect to a Postgre SQL database, must set up connection string before use it.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static DbContext<TOption> UsePostgreSql<TOption>(
            this DbContext<TOption> dbContext) where TOption : class, IDbContextOptions
        {
            // register dbprovider in service collection
            dbContext.ServiceCollection?.TryAddSingleton<INpgsqlDbContext<TOption>, NpgsqlDbContext<TOption>>();

            // register sql factory for create connection, command and dataAdapter
            dbContext.ServiceCollection?.TryAddSingleton<INpgsqlConnectionFactory<TOption>, NpgsqlConnectionFactory<TOption>>();

            return dbContext;
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
        /// <param name="dbContext"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static DbContext<TOption> RegisterPostgreRepositories<TOption>(
            this DbContext<TOption> dbContext, params string[] assemblyName) where TOption : class, IDbContextOptions
        {
            var allAssembly = AppAssembly.GetAll(assemblyName);

            dbContext.ServiceCollection?.RegisterServiceByAttribute(ServiceLifetime.Singleton, allAssembly);
            dbContext.ServiceCollection?.RegisterServiceByAttribute(ServiceLifetime.Scoped, allAssembly);
            dbContext.ServiceCollection?.RegisterServiceByAttribute(ServiceLifetime.Transient, allAssembly);

            return dbContext;
        }

        private static void RegisterServiceByAttribute(this IServiceCollection services, ServiceLifetime serviceLifetime, Assembly[] allAssembly)
        {
            List<Type> types = allAssembly
                .SelectMany(t => 
                t.GetTypes())
                .Where(t => !t.IsInterface && !t.IsSealed && !t.IsAbstract)
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
