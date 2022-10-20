using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki
{
    public class DbContextOptions : IDbContextOptions
    {
        /// <summary>
        /// Specifies the wait time before terminating the attempt to execute a query and generating an error.
        /// </summary>
        public int DbCommandTimeout { get; set; } = 300;

        /// <summary>
        /// Tries n times before throwing an exception
        /// </summary>
        public int NumberOfTries { get; set; } = 5;

        /// <summary>
        /// Preferred gap time to delay before retry
        /// </summary>
        public int DeltaTime { get; set; } = 1;

        /// <summary>
        /// Maximum gap time for each delay time before retry
        /// </summary>
        public int MaxTimeInterval { get; set; } = 20;

        /// <summary>
        /// Gets or sets the string used to open a SQL Server database.
        /// </summary>
        public string ConnectionSting { get; set; } = "";
        public string? DbProviderName { get; set; }

        /// <summary>
        /// Log database execution time and network travel time
        /// </summary>
        public bool EnableStatistics { get; set;}
    }
}
