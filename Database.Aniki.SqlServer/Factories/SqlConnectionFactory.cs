using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Database.Aniki.SqlServer
{
    internal class SqlConnectionFactory<TOption> : ISqlConnectionFactory<TOption> where TOption : class, IDbContextOptions
    {
        private readonly string _sqlConnectionString;

        public SqlConnectionFactory(IOptions<TOption> options)
        {
            _sqlConnectionString = options.Value.ConnectionSting;
        }

        public SqlCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query);
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }

        public SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public SqlDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
