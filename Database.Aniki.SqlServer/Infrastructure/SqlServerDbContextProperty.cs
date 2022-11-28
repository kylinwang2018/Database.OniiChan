using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    public partial class SqlServerDbContext : ISqlServerDbContext
    {
        public RelationalDbOptions Options { 
            get
            {
                return _options;
            }}
    }
}
