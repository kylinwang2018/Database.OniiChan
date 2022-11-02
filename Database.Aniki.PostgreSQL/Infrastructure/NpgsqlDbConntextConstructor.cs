using Database.Aniki.PostgreSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;

namespace Database.Aniki
{
    internal partial class NpgsqlDbContext<TOption> : INpgsqlDbContext<TOption> where TOption : class, IDbContextOptions
    {
        private readonly IOptions<TOption> _option;
        private readonly IDbContextOptions _options;
        private readonly ILogger<NpgsqlDbContext<TOption>> _logger;
        private readonly INpgsqlConnectionFactory<TOption> _connectionFactory;
        private readonly NpgsqlRetryLogicOption _sqlRetryOption;

        public NpgsqlDbContext(
            IOptions<TOption> options, ILogger<NpgsqlDbContext<TOption>> logger,
            INpgsqlConnectionFactory<TOption> connectionFactory)
        {
            _option = options;
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
