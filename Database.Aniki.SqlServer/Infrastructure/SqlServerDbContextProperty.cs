using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    public partial class SqlServerDbContext : IDbContext
    {
        public RelationalDbOptions Options { 
            get
            {
                return _options;
            }}
    }
}
