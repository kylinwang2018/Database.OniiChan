using Database.Aniki.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Aniki.Demo.Models
{
    public class ApplicationDbContext : SqlServerDbContext
    {
        public ApplicationDbContext(
            IOptionsMonitor<RelationalDbOptions> options, 
            ILogger<ApplicationDbContext> logger, 
            ISqlConnectionFactory<ApplicationDbContext, RelationalDbOptions> connectionFactory)
            : base(options, logger, connectionFactory)
        {
        }
    }
}
