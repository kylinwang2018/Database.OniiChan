using Database.Aniki.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database.Aniki
{
    public class MongoDbContext : IDbContext
    {
        protected readonly MongoDbContextOptions _options;
        protected readonly IMongoDbConnectionFactory<MongoDbContext, MongoDbContextOptions> _mongoDbConnectionFactory;

        public MongoDbContext(
            IOptionsMonitor<MongoDbContextOptions> options,
            IMongoDbConnectionFactory<MongoDbContext, MongoDbContextOptions> mongoDbConnectionFactory)
        {
            _options = options.Get((this.GetType()).ToString());
            _mongoDbConnectionFactory = mongoDbConnectionFactory;
        }

        public MongoDbContextOptions Options
        {
            get
            {
                return _options;
            }
        }

        public IMongoDatabase Database
        {
            get
            {
                return _mongoDbConnectionFactory.ConnectDatabase();
            }
        }
    }
}
