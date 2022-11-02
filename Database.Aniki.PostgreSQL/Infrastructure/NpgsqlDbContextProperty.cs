using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    internal partial class NpgsqlDbContext<TOption> : INpgsqlDbContext<TOption> where TOption : class, IDbContextOptions
    {
        public TOption Options
        {
            get
            {
                return _option.Value;
            }
        }
    }
}
