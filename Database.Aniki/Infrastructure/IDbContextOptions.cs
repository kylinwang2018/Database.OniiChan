namespace Database.Aniki
{
    public interface IDbContextOptions
    {
        string ConnectionSting { get; set; }
        int DbCommandTimeout { get; set; }
        string? DbProviderName { get; set; }

        /// <summary>
        /// Preferred gap time to delay before retry
        /// </summary>
        int DeltaTime { get; set; }

        /// <summary>
        /// Maximum gap time for each delay time before retry
        /// </summary>
        int MaxTimeInterval { get; set; }

        /// <summary>
        /// Tries n times before throwing an exception
        /// </summary>
        int NumberOfTries { get; set; }
    }
}