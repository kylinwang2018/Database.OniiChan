﻿using Database.Aniki.MongoDb;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Database.Aniki
{
    public class MongoDbContext : IMongoDbContext
    {
        protected readonly MongoDbContextOptions _options;
        protected readonly IMongoDbConnectionFactory<MongoDbContext, MongoDbContextOptions> _connectionFactory;
        private readonly ILogger<MongoDbContext> _logger;

        public MongoDbContext(
            IMongoDbOptions<MongoDbContext> mongoDbOptions)
        {
            _options = mongoDbOptions.Options.Get((this.GetType()).ToString());
            _connectionFactory = mongoDbOptions.ConnectionFactory;
            _logger = mongoDbOptions.Logger;
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
                return _connectionFactory.ConnectDatabase();
            }
        }
    }
}
