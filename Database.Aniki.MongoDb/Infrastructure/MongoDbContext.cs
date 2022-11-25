using Database.Aniki.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database.Aniki
{
    public class MongoDbContext : IDbContext
    {
        protected readonly MongoDbOptions _options;
        protected readonly IMongoDbConnectionFactory<MongoDbContext, MongoDbOptions> _mongoDbConnectionFactory;

        public MongoDbContext(
            IOptionsMonitor<MongoDbOptions> options,
            IMongoDbConnectionFactory<MongoDbContext, MongoDbOptions> mongoDbConnectionFactory)
        {
            _options = options.Get((this.GetType()).ToString());
            _mongoDbConnectionFactory = mongoDbConnectionFactory;
        }

        public MongoDbOptions Options
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
