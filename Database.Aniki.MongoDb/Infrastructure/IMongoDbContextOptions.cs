namespace Database.Aniki.MongoDb
{
    /// <summary>
    /// The options to be used by a DbContext. 
    /// You normally override this <see cref="MongoDbContextOptions"/> to
    /// create instances of this class and it is not designed to be directly constructed in your application code.
    /// </summary>
    public interface IMongoDbContextOptions : IDbContextOptions
    {
        /// <summary>
        /// The name of your MongoDb database you are going to use.
        /// </summary>
        string DatabaseName { get; set; }
    }
}