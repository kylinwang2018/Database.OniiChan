using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    public class DbContextOptions : IDbContextOptions
    {
        public int DbCommandTimeout { get; set; } = 300;
        public int NumberOfTries { get; set; } = 5;
        public int DeltaTime { get; set; } = 1;
        public int MaxTimeInterval { get; set; } = 20;
        public string ConnectionSting { get; set; } = "";
        public string? DbProviderName { get; set; }
    }
}
