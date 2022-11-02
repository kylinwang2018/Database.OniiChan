using Database.Aniki.MongoDb;
using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Database.Aniki
{
    /// <summary>
    ///     PostgreSql specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class MongoDbServiceCollectionExtensions
    {
        /// <summary>
        ///     Configures the context to connect to a MongoDb database, must set up connection string before use it.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static DbContext<TOption> UseMongoDb<TOption>(
            this DbContext<TOption> dbContext) where TOption : class, IMongoDbContextOptions
        {
            // register dbprovider in service collection
            dbContext.ServiceCollection?.TryAddScoped<IMongoDbContext<TOption>, MongoDbContext<TOption>>();

            dbContext.ServiceCollection?.TryAddScoped<IMongoDbConnectionFactory<TOption>, MongoDbConnectionFactory<TOption>>();

            return dbContext;
        }

        /// <summary>
        /// <para>
        /// inject all project-related repository with [MongoDbRepo]
        /// (<see cref="MongoDbRepoAttribute"/>) attribute to <see cref="IServiceCollection"/>.
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
        /// <param name="assemblyNameStart"></param>
        /// <returns></returns>
        public static DbContext<TOption> RegisterMongoDbRepositories<TOption>(
            this DbContext<TOption> dbContext, string? assemblyNameStart) where TOption : class, IMongoDbContextOptions
        {
            var allAssembly = AppAssembly.GetAll(assemblyNameStart);

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
                    .Where(t =>
                        t.GetCustomAttributes(typeof(MongoDbRepoAttribute), false).Length > 0 &&
                            t.GetCustomAttribute<MongoDbRepoAttribute>()?.Lifetime == serviceLifetime &&
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
