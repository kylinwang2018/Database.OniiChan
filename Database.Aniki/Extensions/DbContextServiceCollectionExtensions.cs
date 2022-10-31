using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Database.Aniki
{
    public static class DbContextServiceCollectionExtensions
    {
        public static DbContext<TOption> AddDbContext<TOption>(
            this IServiceCollection serviceCollection,
            Action<TOption> setupAction) where TOption : class
        {
            Check.NotNull(setupAction, nameof(setupAction));

            // setup options
            serviceCollection.AddOptions();
            serviceCollection.Configure(setupAction);

            return new DbContext<TOption>
            {
                ServiceCollection = serviceCollection
            };
        }
    }

    public class DbContext<TOption> where TOption : class
    {
        public IServiceCollection? ServiceCollection { get; set; }
    }
}
