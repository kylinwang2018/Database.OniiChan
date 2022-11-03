using Database.Aniki.SqlServer;
using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Database.Aniki
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the context to connect to a Microsoft SQL Server database, must set up 
        /// connection string before use it.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static DbContext<TOption> UseSqlServer<TOption>(
            this DbContext<TOption> dbContext) where TOption : class, IDbContextOptions
        {
            // register dbprovider in service collection
            dbContext.ServiceCollection?.TryAddSingleton<ISqlServerDbContext<TOption>, SqlServerDbContext<TOption>>();

            // register sql factory for create connection, command and dataAdapter
            dbContext.ServiceCollection?.TryAddSingleton<ISqlConnectionFactory<TOption>, SqlConnectionFactory<TOption>>();

            return dbContext;
        }

        /// <summary>
        /// <para>
        /// inject all project-related repository with [SqlServerRepo]
        /// (<see cref="SqlServerRepoAttribute"/>) attribute to <see cref="IServiceCollection"/>.
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
        public static DbContext<TOption> RegisterSqlServerRepositories<TOption>(
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

            var types = allAssembly
                .SelectMany(t =>
                    t.GetTypes())
                .Where(t => !t.IsInterface && !t.IsSealed && !t.IsAbstract)
                .Where(s=> s.GetCustomAttributes(typeof(SqlServerRepoAttribute),false).Length > 0 &&
                            s.GetCustomAttribute<SqlServerRepoAttribute>()?.Lifetime == serviceLifetime &&
                            s.IsClass && !s.IsAbstract);

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
