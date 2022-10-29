using Database.Aniki.PostgreSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;

namespace Database.Aniki
{
    internal partial class NpgsqlDbContext<T> : INpgsqlDbContext<T> where T : class, IDbContextOptions
    {
        private readonly IDbContextOptions _options;
        private readonly ILogger<NpgsqlDbContext<T>> _logger;
        private readonly INpgsqlConnectionFactory _connectionFactory;
        private readonly NpgsqlRetryLogicOption _sqlRetryOption;

        public NpgsqlDbContext(
            IOptions<DbContextOptions> options, ILogger<NpgsqlDbContext<T>> logger,
            INpgsqlConnectionFactory connectionFactory)
        {
            _options = options.Value;
            _logger = logger;
            _connectionFactory = connectionFactory;
            _sqlRetryOption = new NpgsqlRetryLogicOption()
            {
                NumberOfTries = _options.NumberOfTries,
                DeltaTime = TimeSpan.FromSeconds(_options.DeltaTime),
            };
        }

        private void LogSqlInfo(NpgsqlCommand sqlCommand, NpgsqlConnection connection)
        {
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
            if (exception is NpgsqlException)
            {

            }
            _logger.LogError("Command:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                query,
                exception.Message,
                exception.StackTrace
                );
        }
    }
}
