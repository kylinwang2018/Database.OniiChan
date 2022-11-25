namespace Database.Aniki.MongoDb
{
    public interface IMongoDbOptions : IDbContextOptions
    {
        string DatabaseName { get; set; }
    }
}