using MongoDB.Driver;

namespace Database.Aniki.MongoDb
{
    public interface IMongoDbContext<TOption> where TOption : class, IMongoDbContextOptions
    {
        TOption Options { get; }
    }
}