using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Aniki.MongoDb
{
    internal class MongoDbContext<TOption> : IMongoDbContext<TOption> where TOption : class, IMongoDbContextOptions
    {
        private readonly TOption _options;
        private readonly IMongoDbConnectionFactory<TOption> _mongoDbConnectionFactory;

        public MongoDbContext(
            IOptions<TOption> options,
            IMongoDbConnectionFactory<TOption> mongoDbConnectionFactory)
        {
            _options = options.Value;
            _mongoDbConnectionFactory = mongoDbConnectionFactory;
        }

        public TOption Options
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
