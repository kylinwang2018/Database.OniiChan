using Database.Aniki.Exceptions;
using Database.Aniki.Utilities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database.Aniki.MongoDb
{
    internal class MongoDbConnectionFactory<TOption> : IMongoDbConnectionFactory<TOption> where TOption : class, IMongoDbContextOptions
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public MongoDbConnectionFactory(IOptions<TOption> options)
        {
            _connectionString = options.Value.ConnectionSting;
            _databaseName = options.Value.DatabaseName;
        }

        public IMongoClient CreateClient()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new DatabaseException("Connection string cannot be empty.");

            return new MongoClient(_connectionString);
        }

        public IMongoDatabase ConnectDatabase(IMongoClient mongoClient)
        {
            if (string.IsNullOrWhiteSpace(_databaseName))
                throw new DatabaseException("Database Name cannot be empty.");

            return mongoClient.GetDatabase(_databaseName);
        }

        public IMongoDatabase ConnectDatabase()
        {
            var client = CreateClient();
            return ConnectDatabase(client);
        }
    }
}
