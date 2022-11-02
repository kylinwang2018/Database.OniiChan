
using MongoDB.Driver;

namespace Database.Aniki.MongoDb
{
    public interface IMongoDbConnectionFactory<TOption> where TOption : class, IMongoDbContextOptions
    {
        IMongoDatabase ConnectDatabase();
        IMongoDatabase ConnectDatabase(IMongoClient mongoClient);
        IMongoClient CreateClient();
    }
}