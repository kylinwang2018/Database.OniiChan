using Database.Aniki.MongoDb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    public interface IMongoDbOptions<out TDbContext> where TDbContext : class, IMongoDbContext
    {
        IMongoDbConnectionFactory<TDbContext, MongoDbContextOptions> ConnectionFactory { get; }
        ILogger<TDbContext> Logger { get; }
        IOptionsMonitor<MongoDbContextOptions> Options { get; }
    }
}