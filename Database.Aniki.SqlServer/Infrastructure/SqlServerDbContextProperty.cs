using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    internal partial class SqlServerDbContext<TOption> : ISqlServerDbContext<TOption> where TOption : class, IDbContextOptions
    {
        public string? ConnectionString { 
            get
            {
                return _options.ConnectionSting;
            }}
    }
}
