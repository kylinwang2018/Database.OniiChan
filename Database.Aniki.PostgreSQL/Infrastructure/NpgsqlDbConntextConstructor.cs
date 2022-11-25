using Database.Aniki.PostgresSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;

namespace Database.Aniki
{
    public partial class NpgsqlDbContext : IDbContext
    {
        private readonly NpgsqlRetryLogicOption _sqlRetryOption;
        protected readonly RelationalDbOptions _options;
        protected readonly ILogger<NpgsqlDbContext> _logger;
        protected readonly INpgsqlConnectionFactory<NpgsqlDbContext, RelationalDbOptions> _connectionFactory;

        public NpgsqlDbContext(
            IOptionsMonitor<RelationalDbOptions> options, ILogger<NpgsqlDbContext> logger,
            INpgsqlConnectionFactory<NpgsqlDbContext, RelationalDbOptions> connectionFactory)
        {
            _options = options.Get((this.GetType()).ToString());
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
