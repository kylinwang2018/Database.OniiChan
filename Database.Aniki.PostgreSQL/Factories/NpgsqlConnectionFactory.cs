using Microsoft.Extensions.Options;
using Npgsql;

namespace Database.Aniki.PostgresSQL
{
    internal class NpgsqlConnectionFactory<TOption> : INpgsqlConnectionFactory<TOption> where TOption : class, IDbContextOptions
    {
        private readonly string _sqlConnectionString;

        public NpgsqlConnectionFactory(IOptions<TOption> options)
        {
            _sqlConnectionString = options.Value.ConnectionSting;
        }

        public NpgsqlCommand CreateCommand()
        {
            return new NpgsqlCommand();
        }

        public NpgsqlCommand CreateCommand(string query)
        {
            return new NpgsqlCommand(query);
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_sqlConnectionString);
        }

        public NpgsqlConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public NpgsqlDataAdapter CreateDataAdapter()
        {
            return new NpgsqlDataAdapter();
        }
    }
}
