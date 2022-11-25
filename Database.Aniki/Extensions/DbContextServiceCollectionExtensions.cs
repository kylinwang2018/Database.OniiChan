using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Database.Aniki
{
    public class DbContext<TDbContext> 
        where TDbContext : class, IDbContext
    {
        public IServiceCollection? ServiceCollection { get; set; }
    }
}
