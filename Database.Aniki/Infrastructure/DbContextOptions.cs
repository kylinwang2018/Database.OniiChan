﻿using System;

namespace Database.Aniki
{
    /// <summary>
    /// The options to be used by a DbContext. 
    /// You normally override this <see cref="DbContextOptions"/> to
    /// create instances of this class and it is not designed to be directly constructed in your application code.
    /// </summary>
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
        public string ConnectionSting
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if (isConnectionStringInitialized)
                    throw new FieldAccessException("Connection string cannot be modified after initialized.");
                _connectionString = value;
                isConnectionStringInitialized = true;
            }
        }

        /// <summary>
        /// Log database execution time and network travel time
        /// </summary>
        public bool EnableStatistics { get; set;}

        private bool isConnectionStringInitialized = false;
        private string _connectionString = string.Empty;
    }
}
