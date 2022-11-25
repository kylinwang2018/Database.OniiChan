﻿
using MongoDB.Driver;

namespace Database.Aniki.MongoDb
{
    public interface IMongoDbConnectionFactory<out TDbContext, TOption>
        where TDbContext : class, IDbContext
        where TOption : class, IMongoDbOptions
    {
        IMongoDatabase ConnectDatabase();
        IMongoDatabase ConnectDatabase(IMongoClient mongoClient);
        IMongoClient CreateClient();
    }
}