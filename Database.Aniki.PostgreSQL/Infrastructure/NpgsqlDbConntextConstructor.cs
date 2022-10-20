using Database.Aniki.PostgreSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;

namespace Database.Aniki
{
    internal partial class NpgsqlDbContext : INpgsqlDbContext
    {
        private readonly IDbContextOptions _options;
        private readonly ILogger<NpgsqlDbContext> _logger;
        private readonly INpgsqlConnectionFactory _connectionFactory;
        private readonly SqlRetryLogicBaseProvider _sqlRetryProvider;

        public NpgsqlDbContext(
            IOptions<DbContextOptions> options, ILogger<NpgsqlDbContext> logger,
            INpgsqlConnectionFactory connectionFactory)
        {
            _options = options.Value;
            _logger = logger;
            _connectionFactory = connectionFactory;
            _sqlRetryProvider = NpgsqlRetryingExecutionStrategy
                .CreateExponentialRetryProvider(new SqlRetryLogicOption()
                {
                    NumberOfTries = _options.NumberOfTries,
                    DeltaTime = TimeSpan.FromSeconds(_options.DeltaTime),
                    MaxTimeInterval = TimeSpan.FromSeconds(_options.MaxTimeInterval)
                });
        }

        private void LogSqlInfo(NpgsqlCommand sqlCommand, NpgsqlConnection connection)
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

        private void LogSqlError(NpgsqlCommand sqlCommand, Exception exception)
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
