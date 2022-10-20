﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    public partial class SqlServerDbContext : ISqlServerDbContext
    {
        private readonly IDbContextOptions _options;
        private readonly ILogger<SqlServerDbContext> _logger;
        private readonly SqlRetryLogicBaseProvider _sqlRetryProvider;

        public SqlServerDbContext(
            IOptions<DbContextOptions> options, ILogger<SqlServerDbContext> logger)
        {
            _options = options.Value;
            _logger = logger;

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
            var stats = connection.RetrieveStatistics();
            var executionTime = (long)stats["ExecutionTime"];
            var commandNetworkServerTimeInMs = (long)stats["NetworkServerTime"];
            _logger.LogInformation("Command:\n\t{Command}\nExecution Time: {Time}[ms]\nNetwork Time: {NetworkTime}[ms]",
                sqlCommand.CommandText,
                executionTime,
                commandNetworkServerTimeInMs
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
