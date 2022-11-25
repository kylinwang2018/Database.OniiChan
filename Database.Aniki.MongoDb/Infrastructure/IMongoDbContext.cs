using Database.Aniki.MongoDb;
using MongoDB.Driver;

namespace Database.Aniki
{
    public interface IMongoDbContext<TOption> where TOption : class, IMongoDbOptions
    {
        TOption Options { get; }
        IMongoDatabase Database { get; }
    }
}