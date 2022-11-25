using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Database.Aniki.SqlServer
{
    internal class SqlConnectionFactory<TDbContext, TOption> : ISqlConnectionFactory<TDbContext, TOption>
        where TOption : class, IDbContextOptions
        where TDbContext : class, IDbContext
    {
        private readonly string _sqlConnectionString;

        public SqlConnectionFactory(IOptionsMonitor<TOption> options)
        {
            _sqlConnectionString = options.Get(typeof(TDbContext).ToString()).ConnectionSting;
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
