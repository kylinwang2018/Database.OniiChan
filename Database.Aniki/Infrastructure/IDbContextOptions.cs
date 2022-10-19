namespace Database.Aniki
{
    public interface IDbContextOptions
    {
        int DbCommandTimeout { get; set; }
        int MaximunRetryTime { get; set; }
        int RetryWaitingSeconds { get; set; }
        string ConnectionSting { get; set; }
        string? DbProviderName { get; set; }
    }
}