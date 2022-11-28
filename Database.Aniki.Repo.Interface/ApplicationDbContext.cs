using Database.Aniki.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Aniki.Demo.Models
{
    public class ApplicationDbContext : SqlServerDbContext
    {
        public ApplicationDbContext(ISqlServerOptions<ApplicationDbContext> sqlServerOptions) : base(sqlServerOptions)
        {
        }
    }
}
