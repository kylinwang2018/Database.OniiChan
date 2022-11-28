using Database.Aniki.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Database.Aniki
{
    public partial class SqlServerDbContext : ISqlServerDbContext
    {
        protected readonly SqlRetryLogicBaseProvider _sqlRetryProvider;
        protected readonly RelationalDbOptions _options;
        protected readonly ILogger<SqlServerDbContext> _logger;
        protected readonly ISqlConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;

        public SqlServerDbContext(
            ISqlServerOptions<SqlServerDbContext> sqlServerOptions)
        {
            _options = sqlServerOptions.Options.Get((this.GetType()).ToString());
            _logger = sqlServerOptions.Logger;
            _connectionFactory = sqlServerOptions.ConnectionFactory;
            _sqlRetryProvider = SqlConfigurableRetryFactory
                .CreateExponentialRetryProvider(new SqlRetryLogicOption()
                {
                    NumberOfTries = _options.NumberOfTries,
                    DeltaTime = TimeSpan.FromSeconds(_options.DeltaTime),
                    MaxTimeInterval = TimeSpan.FromSeconds(_options.MaxTimeInterval)
                });
        }

        private void LogSqlInfo(SqlCommand sqlCommand, SqlConnection connection)
        {
            if (_options.EnableStatistics)
            {
                var stats = connection.RetrieveStatistics();
                var executionTime = (long)stats["ExecutionTime"];
                var commandNetworkServerTimeInMs = (long)stats["NetworkServerTime"];
                _logger.LogInformation("Command:\n\t{Command}\nExecution Time: {Time}[ms]\nNetwork Time: {NetworkTime}[ms]",
                    sqlCommand.CommandText,
                    executionTime,
                    commandNetworkServerTimeInMs
                    );
            }
            else
                _logger.LogInformation("Command:\n\t{Command}",
                    sqlCommand.CommandText
                    );

        }

        private void LogSqlError(SqlCommand sqlCommand, Exception exception)
        {
            _logger.LogError("Command:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        private void LogSqlError(string query, Exception exception)
        {
            _logger.LogError("Command:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                query,
                exception.Message,
                exception.StackTrace
                );
        }
    }
}
