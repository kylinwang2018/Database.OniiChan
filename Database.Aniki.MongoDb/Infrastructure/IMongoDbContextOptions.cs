namespace Database.Aniki.MongoDb
{
    public interface IMongoDbContextOptions : IDbContextOptions
    {
        string DatabaseName { get; set; }
    }
}