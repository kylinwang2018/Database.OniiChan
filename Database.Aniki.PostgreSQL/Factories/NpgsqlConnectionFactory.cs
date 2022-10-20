using Microsoft.Extensions.Options;
using Npgsql;

namespace Database.Aniki.PostgreSQL
{
    internal class NpgsqlConnectionFactory : INpgsqlConnectionFactory
    {
        private readonly string _sqlConnectionString;

        public NpgsqlConnectionFactory(IOptions<DbContextOptions> options)
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
    {
    }
}
