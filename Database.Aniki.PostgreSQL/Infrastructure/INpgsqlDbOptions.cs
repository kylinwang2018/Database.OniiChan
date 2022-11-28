using Database.Aniki.PostgresSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    public interface INpgsqlDbOptions<out TDbContext>
        where TDbContext : class, INpgsqlDbContext
    {
        INpgsqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
        ILogger<TDbContext> Logger { get; }
        IOptionsMonitor<RelationalDbOptions> Options { get; }
    }
}