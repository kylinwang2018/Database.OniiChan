using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Database.Aniki
{
    public static class DbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext<T>(
            this IServiceCollection serviceCollection,
            Action<T> setupAction) where T : class
        {
            Check.NotNull(setupAction, nameof(setupAction));

            // setup options
            serviceCollection.AddOptions();
            serviceCollection.Configure(setupAction);

            return serviceCollection;
        }
    }
}
