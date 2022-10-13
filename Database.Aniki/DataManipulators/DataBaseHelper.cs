using Database.Aniki.Exceptions;
using Database.Aniki.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Database.Aniki.DataManipulators
{
    internal static class DataBaseHelper
    {
        public static DbConnection CreateDbConnection(string providerName, string connectionString)
        {
            if (connectionString != null)
            {
                var factory = DbProviderFactories.GetFactory(providerName);
                var dbConnection = factory.CreateConnection();
                dbConnection.ConnectionString = connectionString;
                return dbConnection;
            }
            else
                throw new DatabaseException();
        }

        public static int ExecuteNonQuery(DbCommand cmd, bool closeWhenComplete = false, int dbCommandTimeout = 30)
        {
            DbConnection connection = cmd.Connection;
            cmd.CommandTimeout = dbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            int result = cmd.ExecuteNonQuery();
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(DbCommand cmd, bool closeWhenComplete = false, int dbCommandTimeout = 30)
        {
            DbConnection connection = cmd.Connection;
            cmd.CommandTimeout = dbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            int result = await cmd.ExecuteNonQueryAsync();
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string providerName, string connectionString, int dbCommandTimeout = 30)
        {
            int result;
            using (DbConnection dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using DbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = dbCommandTimeout;
                dbConnection.Open();
                int num = dbCommand.ExecuteNonQuery();
                result = num;
            }
            return result;
        }
        public static async Task<int> ExecuteNonQueryAsync(string query, string providerName, string connectionString, int dbCommandTimeout = 30)
        {
            int result;
            using (DbConnection dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using DbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = dbCommandTimeout;
                await dbConnection.OpenAsync();
                var num = await dbCommand.ExecuteNonQueryAsync();
                result = num;
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, DbConnection connection, bool closeWhenComplete = false, int dbCommandTimeout = 30)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            int result;
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = dbCommandTimeout;
                if (closeWhenComplete)
                {
                    connection.Close();
                }
                int num = dbCommand.ExecuteNonQuery();
                result = num;
            }
            return result;
        }
        public static async Task<int> ExecuteNonQueryAsync(string query, DbConnection connection, bool closeWhenComplete = false, int dbCommandTimeout = 30)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            int result;
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = dbCommandTimeout;
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
                int num = await dbCommand.ExecuteNonQueryAsync();
                result = num;
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, int dbCommandTimeout = 30)
        {
            int result;
            using (DbConnection dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using (DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = query;
                    dbCommand.CommandTimeout = dbCommandTimeout;
                    foreach (DbParam dbParam in parameters)
                    {
                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = dbParam.ParameterName;
                        dbParameter.Value = dbParam.Value;
                        dbParameter.DbType = dbParam.Type;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                    dbConnection.Open();
                    int num = dbCommand.ExecuteNonQuery();
                    result = num;
                }
            }
            return result;
        }
    }
}
