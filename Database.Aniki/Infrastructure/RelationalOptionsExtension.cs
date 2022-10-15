using Database.Aniki.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Database.Aniki.Infrastructure
{
    /// <summary>
    ///     <para>
    ///         Represents options managed by the relational database providers.
    ///         These options are set using <see cref="DbContextOptionsBuilder" />.
    ///     </para>
    ///     <para>
    ///         Instances of this class are designed to be immutable. To change an option, call one of the 'With...'
    ///         methods to obtain a new instance with the option changed.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     See <see href="https://aka.ms/efcore-docs-providers">Implementation of database providers and extensions</see>
    ///     for more information.
    /// </remarks>
    public abstract class RelationalOptionsExtension : IDbContextOptionsExtension
    {
        // NB: When adding new options, make sure to update the copy constructor below.
        private string? _connectionString;
        private DbConnection? _connection;
        private int? _commandTimeout;
        private int? _maxBatchSize;
        private int? _minBatchSize;
        private bool _useRelationalNulls;
        private string? _migrationsAssembly;
        private string? _migrationsHistoryTableName;
        private string? _migrationsHistoryTableSchema;

        /// <summary>
        ///     Creates a new set of options with everything set to default values.
        /// </summary>
        protected RelationalOptionsExtension()
        {
        }

        /// <summary>
        ///     Called by a derived class constructor when implementing the <see cref="Clone" /> method.
        /// </summary>
        /// <param name="copyFrom">The instance that is being cloned.</param>
        protected RelationalOptionsExtension(RelationalOptionsExtension copyFrom)
        {
            Check.NotNull(copyFrom, nameof(copyFrom));

            _connectionString = copyFrom._connectionString;
            _connection = copyFrom._connection;
            _commandTimeout = copyFrom._commandTimeout;
            _maxBatchSize = copyFrom._maxBatchSize;
            _minBatchSize = copyFrom._minBatchSize;
            _useRelationalNulls = copyFrom._useRelationalNulls;
            _migrationsAssembly = copyFrom._migrationsAssembly;
            _migrationsHistoryTableName = copyFrom._migrationsHistoryTableName;
            _migrationsHistoryTableSchema = copyFrom._migrationsHistoryTableSchema;
        }

        /// <summary>
        ///     Information/metadata about the extension.
        /// </summary>
        public abstract DbContextOptionsExtensionInfo Info { get; }

        /// <summary>
        ///     Override this method in a derived class to ensure that any clone created is also of that class.
        /// </summary>
        /// <returns>A clone of this instance, which can be modified before being returned as immutable.</returns>
        protected abstract RelationalOptionsExtension Clone();

        /// <summary>
        ///     The connection string, or <see langword="null" /> if a <see cref="DbConnection" /> was used instead of
        ///     a connection string.
        /// </summary>
        public virtual string? ConnectionString
            => _connectionString;

        /// <summary>
        ///     Creates a new instance with all options the same as for this instance, but with the given option changed.
        ///     It is unusual to call this method directly. Instead use <see cref="DbContextOptionsBuilder" />.
        /// </summary>
        /// <param name="connectionString">The option to change.</param>
        /// <returns>A new instance with the option changed.</returns>
        public virtual RelationalOptionsExtension WithConnectionString(string? connectionString)
        {
            Check.NullButNotEmpty(connectionString, nameof(connectionString));

            var clone = Clone();

            clone._connectionString = connectionString;

            return clone;
        }

        /// <summary>
        ///     The <see cref="DbConnection" />, or <see langword="null" /> if a connection string was used instead of
        ///     the full connection object.
        /// </summary>
        public virtual DbConnection? Connection
            => _connection;

        /// <summary>
        ///     Creates a new instance with all options the same as for this instance, but with the given option changed.
        ///     It is unusual to call this method directly. Instead use <see cref="DbContextOptionsBuilder" />.
        /// </summary>
        /// <param name="connection">The option to change.</param>
        /// <returns>A new instance with the option changed.</returns>
        public virtual RelationalOptionsExtension WithConnection(DbConnection? connection)
        {
            var clone = Clone();

            clone._connection = connection;

            return clone;
        }

        /// <summary>
        ///     Adds the services required to make the selected options work. This is used when there
        ///     is no external <see cref="IServiceProvider" /> and EF is maintaining its own service
        ///     provider internally. This allows database providers (and other extensions) to register their
        ///     required services when EF is creating an service provider.
        /// </summary>
        /// <param name="services">The collection to add services to.</param>
        public abstract void ApplyServices(IServiceCollection services);

        /// <summary>
        ///     Gives the extension a chance to validate that all options in the extension are valid.
        ///     Most extensions do not have invalid combinations and so this will be a no-op.
        ///     If options are invalid, then an exception should be thrown.
        /// </summary>
        /// <param name="options">The options being validated.</param>
        public virtual void Validate(IDbContextOptions options)
        {
        }
    }
}
