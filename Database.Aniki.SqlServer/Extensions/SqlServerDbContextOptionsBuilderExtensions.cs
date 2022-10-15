using Database.Aniki.Infrastructure;
using System;
using Database.Aniki.Utilities;
using System.Collections.Generic;
using System.Text;
using Database.Aniki.SqlServer.Infrastructure;

namespace Database.Aniki
{
    /// <summary>
    /// Provides extension methods on <see cref="DbContextOptionsBuilder"/> and <see cref="DbContextOptionsBuilder{T}"/>
    /// used to configure a <see cref="DbContext"/> to context to a PostgreSQL database with Npgsql.
    /// </summary>
    public static class SqlServerDbContextOptionsBuilderExtensions
    {
        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
        ///     <see href="https://aka.ms/efcore-docs-sqlserver">Accessing SQL Server and SQL Azure databases with EF Core</see>
        ///     for more information.
        /// </remarks>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="connectionString">The connection string of the database to connect to.</param>
        /// <param name="sqlServerOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServer(
            this DbContextOptionsBuilder optionsBuilder,
            string connectionString,
            Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotEmpty(connectionString, nameof(connectionString));

            var extension = (SqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            ConfigureWarnings(optionsBuilder);

            sqlServerOptionsAction?.Invoke(new SqlServerDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        private static SqlServerOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>()
                ?? new SqlServerOptionsExtension();

        private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
        {
            CoreOptionsExtension coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
            coreOptionsExtension = RelationalOptionsExtension.WithDefaultWarningConfiguration(coreOptionsExtension).WithWarningsConfiguration(coreOptionsExtension.WarningsConfiguration.TryWithExplicit(SqlServerEventId.ConflictingValueGenerationStrategiesWarning, WarningBehavior.Throw));
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
        }
    }
}
