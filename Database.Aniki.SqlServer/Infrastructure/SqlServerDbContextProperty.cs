using Microsoft.Extensions.Options;

namespace Database.Aniki
{
    internal partial class SqlServerDbContext<TOption> : ISqlServerDbContext<TOption> where TOption : class, IDbContextOptions
    {
        public IOptions<TOption> Options { 
            get
            {
                return _option;
            }}
    }
}
