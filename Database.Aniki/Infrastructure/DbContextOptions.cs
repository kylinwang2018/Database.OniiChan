using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    public class DbContextOptions : IDbContextOptions
    {
        public int DbCommandTimeout { get; set; } = 300;
        public int MaximunRetryTime { get; set; } = 5;
        public int RetryWaitingSeconds { get; set; } = 5;
        public string ConnectionSting { get; set; } = "";
        public string? DbProviderName { get; set; }
    }
}
