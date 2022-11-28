using Database.Aniki.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    internal class SqlServerOptions<TDbContext> : ISqlServerOptions<TDbContext>
        where TDbContext : class, ISqlServerDbContext
    {
        public SqlServerOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            ILogger<TDbContext> logger,
            ISqlConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public ILogger<TDbContext> Logger { get; }
        public ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
    }
}