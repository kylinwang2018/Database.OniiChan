using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    public partial class NpgsqlDbContext : IDbContext
    {
        public RelationalDbOptions Options
        {
            get
            {
                return _options;
            }
        }
    }
}
