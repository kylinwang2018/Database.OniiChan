using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    public static class DbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(
            this IServiceCollection serviceCollection,
            Action<DbContextOptions> setupAction)
        {
            Check.NotNull(setupAction, nameof(setupAction));

            // setup options
            serviceCollection.AddOptions();
            serviceCollection.Configure(setupAction);

            return serviceCollection;
        }
    }
}
