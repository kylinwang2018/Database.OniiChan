using Database.Aniki.Infrastructure;
using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Database.Aniki
{
    /// <summary>
    ///     Extension methods for setting up Database Aniki related services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext<TImplementInterface>(
            this IServiceCollection serviceCollection,
            Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            if (contextLifetime == ServiceLifetime.Singleton)
            {
                optionsLifetime = ServiceLifetime.Singleton;
            }

            AddCoreServices<TContextImplementation>(serviceCollection, optionsAction, optionsLifetime);

            if (serviceCollection.Any(d => d.ServiceType == typeof(IDbContextFactorySource<TContextImplementation>)))
            {
                // Override registration made by AddDbContextFactory
                var serviceDescriptor = serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(TContextImplementation));
                if (serviceDescriptor != null)
                {
                    serviceCollection.Remove(serviceDescriptor);
                }
            }

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContextService), typeof(TContextImplementation), contextLifetime));

            if (typeof(TContextService) != typeof(TContextImplementation))
            {
                serviceCollection.TryAdd(
                    new ServiceDescriptor(
                        typeof(TContextImplementation),
                        p => (TContextImplementation)p.GetService<TContextService>()!,
                        contextLifetime));
            }

            return serviceCollection;
        }

        private static void AddCoreServices<TContextImplementation>(
            IServiceCollection serviceCollection,
            Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime optionsLifetime)
            where TContextImplementation : DbContext
        {
            serviceCollection.TryAdd(
                new ServiceDescriptor(
                    typeof(DbContextOptions<TContextImplementation>),
                    p => CreateDbContextOptions<TContextImplementation>(p, optionsAction),
                    optionsLifetime));

            serviceCollection.Add(
                new ServiceDescriptor(
                    typeof(DbContextOptions),
                    p => p.GetRequiredService<DbContextOptions<TContextImplementation>>(),
                    optionsLifetime));
        }
    }
}
