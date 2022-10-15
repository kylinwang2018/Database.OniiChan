using Database.Aniki.SqlServer.Infrastructure;
using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Database.Aniki.SqlServer.Extensions
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        ///     <para>
        ///         Registers the given Entity Framework <see cref="DbContext" /> as a service in the <see cref="IServiceCollection" />
        ///         and configures it to connect to a SQL Server database.
        ///     </para>
        ///     <para>
        ///         This method is a shortcut for configuring a <see cref="DbContext" /> to use SQL Server. It does not support all options.
        ///         Use <see cref="O:EntityFrameworkServiceCollectionExtensions.AddDbContext" /> and related methods for full control of
        ///         this process.
        ///     </para>
        ///     <para>
        ///         Use this method when using dependency injection in your application, such as with ASP.NET Core.
        ///         For applications that don't use dependency injection, consider creating <see cref="DbContext" />
        ///         instances directly with its constructor. The <see cref="DbContext.OnConfiguring" /> method can then be
        ///         overridden to configure the SQL Server provider and connection string.
        ///     </para>
        ///     <para>
        ///         To configure the <see cref="DbContextOptions{TContext}" /> for the context, either override the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context, or supply
        ///         an optional action to configure the <see cref="DbContextOptions" /> for the context.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-di">Using DbContext with dependency injection</see> for more information.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
        ///     <see href="https://aka.ms/efcore-docs-sqlserver">Accessing SQL Server and SQL Azure databases with EF Core</see>
        ///     for more information.
        /// </remarks>
        /// <typeparam name="TContext">The type of context to be registered.</typeparam>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="connectionString">The connection string of the database to connect to.</param>
        /// <param name="sqlServerOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <param name="optionsAction">An optional action to configure the <see cref="DbContextOptions" /> for the context.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddSqlServer(
            this IServiceCollection serviceCollection,
            string connectionString,
            Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null,
            Action<DbContextOptionsBuilder>? optionsAction = null)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotEmpty(connectionString, nameof(connectionString));

            return serviceCollection.AddDbContext(
                (serviceProvider, options) =>
                {
                    optionsAction?.Invoke(options);
                    options.UseSqlServer(connectionString, sqlServerOptionsAction);
                });
        }

        /// <summary>
        ///     <para>
        ///         Adds the services required by the Microsoft SQL Server database provider for Entity Framework
        ///         to an <see cref="IServiceCollection" />.
        ///     </para>
        ///     <para>
        ///         Warning: Do not call this method accidentally. It is much more likely you need
        ///         to call <see cref="AddSqlServer{TContext}" />.
        ///     </para>
        ///     <para>
        ///         Calling this method is no longer necessary when building most applications, including those that
        ///         use dependency injection in ASP.NET or elsewhere.
        ///         It is only needed when building the internal service provider for use with
        ///         the <see cref="DbContextOptionsBuilder.UseInternalServiceProvider" /> method.
        ///         This is not recommend other than for some advanced scenarios.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection serviceCollection)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            serviceCollection

            return serviceCollection;
        }
    }
}
