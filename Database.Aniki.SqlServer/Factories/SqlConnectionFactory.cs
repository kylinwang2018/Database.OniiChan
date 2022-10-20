using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Database.Aniki.SqlServer
{
    internal class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _sqlConnectionString;

        public SqlConnectionFactory(IOptions<DbContextOptions> options)
        {
            _sqlConnectionString = options.Value.ConnectionSting;
        }

        public SqlCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }
    }
}
