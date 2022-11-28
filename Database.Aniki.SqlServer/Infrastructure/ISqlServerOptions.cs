using Database.Aniki.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    public interface ISqlServerOptions<out TDbContext>
        where TDbContext : class, ISqlServerDbContext
    {
        ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
        ILogger<TDbContext> Logger { get; }
        IOptionsMonitor<RelationalDbOptions> Options { get; }
    }
}