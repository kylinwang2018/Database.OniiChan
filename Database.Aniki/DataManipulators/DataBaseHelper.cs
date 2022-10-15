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
    internal static class DatabaseHelper
    {
        public static DbConnection CreateDbConnection(string providerName, string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                var factory = DbProviderFactories.GetFactory(providerName);
                var dbConnection = factory.CreateConnection();
                dbConnection.ConnectionString = connectionString;
                return dbConnection;
            }
            else
                throw new DatabaseException();
        }

        public static int ExecuteNonQuery(DbCommand cmd, bool closeWhenComplete = false)
        {
            var connection = cmd.Connection;
            cmd.CommandTimeout = DbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            var result = cmd.ExecuteNonQuery();
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(DbCommand cmd, bool closeWhenComplete = false)
        {
            var connection = cmd.Connection;
            cmd.CommandTimeout = DbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            var result = await cmd.ExecuteNonQueryAsync();
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string providerName, string connectionString)
        {
            int result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                dbConnection.Open();
                result = dbCommand.ExecuteNonQuery();
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, string providerName, string connectionString)
        {
            int result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                await dbConnection.OpenAsync();
                result = await dbCommand.ExecuteNonQueryAsync();
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, DbConnection connection, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            int result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                result = dbCommand.ExecuteNonQuery();
                if (closeWhenComplete)
                {
                    connection.Close();
                }
            }
            return result;
        }
        public static async Task<int> ExecuteNonQueryAsync(string query, DbConnection connection, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            int result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                result = await dbCommand.ExecuteNonQueryAsync();
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters)
        {
            int result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                dbConnection.Open();
                result = dbCommand.ExecuteNonQuery();
            }
            return result;
        }
        public static async Task<int> ExecuteNonQueryAsync(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters)
        {
            int result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                await dbConnection.OpenAsync();
                result = await dbCommand.ExecuteNonQueryAsync();
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, DbConnection connection, IEnumerable<DbParam> parameters, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            int result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                result = dbCommand.ExecuteNonQuery();
                if (closeWhenComplete)
                {
                    connection.Close();
                }
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, DbConnection connection, IEnumerable<DbParam> parameters, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            int result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                result = await dbCommand.ExecuteNonQueryAsync();
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
            }
            return result;
        }

        public static DataTable GetDataTable(DbCommand cmd, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
            var connection = cmd.Connection;
            cmd.CommandTimeout = DbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            var dbDataReader = cmd.ExecuteReader();
            var schemaTable = dbDataReader.GetSchemaTable();
            var list = new List<DataColumn>();
            if (schemaTable != null)
            {
                foreach (DataRow dataRow in schemaTable.Rows)
                {
                    var columnName = Convert.ToString(dataRow["ColumnName"]);
                    var dataColumn = new DataColumn(columnName, (Type)dataRow["DataType"]);
                    list.Add(dataColumn);
                    dataTable.Columns.Add(dataColumn);
                }
            }
            while (dbDataReader.Read())
            {
                var dataRow2 = dataTable.NewRow();
                for (int i = 0; i < list.Count; i++)
                {
                    dataRow2[list[i]] = dbDataReader[i];
                }
                dataTable.Rows.Add(dataRow2);
            }
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(DbCommand cmd, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
            var connection = cmd.Connection;
            cmd.CommandTimeout = DbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            var dbDataReader = await cmd.ExecuteReaderAsync();
            var schemaTable = dbDataReader.GetSchemaTable();
            var list = new List<DataColumn>();
            if (schemaTable != null)
            {
                foreach (DataRow dataRow in schemaTable.Rows)
                {
                    var columnName = Convert.ToString(dataRow["ColumnName"]);
                    var dataColumn = new DataColumn(columnName, (Type)dataRow["DataType"]);
                    list.Add(dataColumn);
                    dataTable.Columns.Add(dataColumn);
                }
            }
            while (await dbDataReader.ReadAsync())
            {
                var dataRow2 = dataTable.NewRow();
                for (int i = 0; i < list.Count; i++)
                {
                    dataRow2[list[i]] = dbDataReader[i];
                }
                dataTable.Rows.Add(dataRow2);
            }
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return dataTable;
        }
        public static DataTable GetDataTable(string query, string providerName, string connectionString)
        {
            var dataTable = new DataTable();
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                dbConnection.Open();
                var dbDataReader = dbCommand.ExecuteReader();
                var schemaTable = dbDataReader.GetSchemaTable();
                var list = new List<DataColumn>();
                if (schemaTable != null)
                {
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        var columnName = Convert.ToString(dataRow["ColumnName"]);
                        var dataColumn = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        list.Add(dataColumn);
                        dataTable.Columns.Add(dataColumn);
                    }
                }
                while (dbDataReader.Read())
                {
                    var dataRow2 = dataTable.NewRow();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dataRow2[list[i]] = dbDataReader[i];
                    }
                    dataTable.Rows.Add(dataRow2);
                }
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string providerName, string connectionString)
        {
            var dataTable = new DataTable();
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                await dbConnection.OpenAsync();
                var dbDataReader = await dbCommand.ExecuteReaderAsync();
                var schemaTable = dbDataReader.GetSchemaTable();
                var list = new List<DataColumn>();
                if (schemaTable != null)
                {
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        var columnName = Convert.ToString(dataRow["ColumnName"]);
                        var dataColumn = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        list.Add(dataColumn);
                        dataTable.Columns.Add(dataColumn);
                    }
                }
                while (await dbDataReader.ReadAsync())
                {
                    var dataRow2 = dataTable.NewRow();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dataRow2[list[i]] = dbDataReader[i];
                    }
                    dataTable.Rows.Add(dataRow2);
                }
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters)
        {
            var dataTable = new DataTable();
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                dbConnection.Open();
                var dbDataReader = dbCommand.ExecuteReader();
                var schemaTable = dbDataReader.GetSchemaTable();
                var list = new List<DataColumn>();
                if (schemaTable != null)
                {
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        var columnName = Convert.ToString(dataRow["ColumnName"]);
                        var dataColumn = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        list.Add(dataColumn);
                        dataTable.Columns.Add(dataColumn);
                    }
                }
                while (dbDataReader.Read())
                {
                    var dataRow = dataTable.NewRow();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dataRow[list[i]] = dbDataReader[i];
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters)
        {
            var dataTable = new DataTable();
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                await dbConnection.OpenAsync();
                var dbDataReader = await dbCommand.ExecuteReaderAsync();
                var schemaTable = dbDataReader.GetSchemaTable();
                var list = new List<DataColumn>();
                if (schemaTable != null)
                {
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        var columnName = Convert.ToString(dataRow["ColumnName"]);
                        var dataColumn = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        list.Add(dataColumn);
                        dataTable.Columns.Add(dataColumn);
                    }
                }
                while (await dbDataReader.ReadAsync())
                {
                    var dataRow = dataTable.NewRow();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dataRow[list[i]] = dbDataReader[i];
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        public static object GetScalar(DbCommand cmd, bool closeWhenComplete = false)
        {
            var connection = cmd.Connection;
            cmd.CommandTimeout = DbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            var result = cmd.ExecuteScalar();
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(DbCommand cmd, bool closeWhenComplete = false)
        {
            var connection = cmd.Connection;
            cmd.CommandTimeout = DbCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            var result = await cmd.ExecuteScalarAsync();
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return result;
        }

        public static object GetScalar(string query, string providerName, string connectionString)
        {
            object result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                dbConnection.Open();
                result = dbCommand.ExecuteScalar();
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(string query, string providerName, string connectionString)
        {
            object result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                await dbConnection.OpenAsync();
                result = await dbCommand.ExecuteScalarAsync();
            }
            return result;
        }

        public static object GetScalar(string query, DbConnection connection, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            object result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                result = dbCommand.ExecuteScalar();
                if (closeWhenComplete)
                {
                    connection.Close();
                }
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(string query, DbConnection connection, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            object result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                result = await dbCommand.ExecuteScalarAsync();
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
            }
            return result;
        }

        public static object GetScalar(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters)
        {
            object result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                dbConnection.Open();
                result = dbCommand.ExecuteScalar();
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters)
        {
            object result;
            using (var dbConnection = CreateDbConnection(providerName, connectionString))
            {
                using var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                await dbConnection.OpenAsync();
                result = await dbCommand.ExecuteScalarAsync();
            }
            return result;
        }

        public static object GetScalar(string query, DbConnection connection, IEnumerable<DbParam> parameters, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            object result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                result = dbCommand.ExecuteScalar();
                if (closeWhenComplete)
                {
                    connection.Close();
                }
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(string query, DbConnection connection, IEnumerable<DbParam> parameters, bool closeWhenComplete = false)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            object result;
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.CommandTimeout = DbCommandTimeout;
                foreach (var dbParam in parameters)
                {
                    var dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = dbParam.ParameterName;
                    dbParameter.Value = dbParam.Value;
                    dbParameter.DbType = dbParam.Type;
                    dbCommand.Parameters.Add(dbParameter);
                }
                result = await dbCommand.ExecuteScalarAsync();
                if (closeWhenComplete)
                {
                    await connection.CloseAsync();
                }
            }
            return result;
        }

        public static List<T> GetDataTable<T>(DbCommand cmd, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(cmd, closeWhenComplete));
        }

        public static List<T> GetDataTable<T>(string query, string providerName, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, providerName, connectionString));
        }

        public static List<T> GetDataTable<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, providerName, connectionString, parameters));
        }

        public static T? GetDataRow<T>(DbCommand cmd, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(cmd, closeWhenComplete));
        }

        public static T? GetDataRow<T>(string query, string providerName, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, providerName, connectionString));
        }

        public static T? GetDataRow<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, providerName, connectionString, parameters));
        }

        public static List<string> GetColumnToString(DbCommand cmd, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, closeWhenComplete), columnIndex);
        }

        public static List<string> GetColumnToString(string query, string providerName, string connectionString, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(query, providerName, connectionString), columnIndex);
        }

        public static List<string> GetColumnToString(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(query, providerName, connectionString, parameters), columnIndex);
        }

        public static List<string> GetColumnToString(DbCommand cmd, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, closeWhenComplete), columnName);
        }

        public static List<string> GetColumnToString(string query, string providerName, string connectionString, string columnName)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(query, providerName, connectionString), columnName);
        }

        public static List<string> GetColumnToString(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, string columnName)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(query, providerName, connectionString, parameters), columnName);
        }

        public static List<T> GetColumn<T>(DbCommand cmd, int columnIndex = 0, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, closeWhenComplete), columnIndex);
        }

        public static List<T> GetColumn<T>(string query, string providerName, string connectionString, int columnIndex = 0) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(query, providerName, connectionString), columnIndex);
        }

        public static List<T> GetColumn<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, int columnIndex = 0) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(query, providerName, connectionString, parameters), columnIndex);
        }

        public static List<T> GetColumn<T>(DbCommand cmd, string columnName, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, closeWhenComplete), columnName);
        }

        public static List<T> GetColumn<T>(string query, string providerName, string connectionString, string columnName) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(query, providerName, connectionString), columnName);
        }

        public static List<T> GetColumn<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, string columnName) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(query, providerName, connectionString, parameters), columnName);
        }

        public static async Task<List<T>> GetDataTableAsync<T>(DbCommand cmd, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(cmd, closeWhenComplete));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string providerName, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, providerName, connectionString));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, providerName, connectionString, parameters));
        }

        public static async Task<T?> GetDataRowAsync<T>(DbCommand cmd, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(cmd, closeWhenComplete));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string providerName, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, providerName, connectionString));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, providerName, connectionString, parameters));
        }

        public static async Task<List<string>> GetColumnToStringAsync(DbCommand cmd, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, closeWhenComplete), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(string query, string providerName, string connectionString, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(query, providerName, connectionString), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(query, providerName, connectionString, parameters), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(DbCommand cmd, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, closeWhenComplete), columnName);
        }

        public static async Task<List<string>> GetColumnToStringAsync(string query, string providerName, string connectionString, string columnName)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(query, providerName, connectionString), columnName);
        }

        public static async Task<List<string>> GetColumnToStringAsync(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, string columnName)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(query, providerName, connectionString, parameters), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(DbCommand cmd, int columnIndex = 0, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, closeWhenComplete), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(string query, string providerName, string connectionString, int columnIndex = 0) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(query, providerName, connectionString), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, int columnIndex = 0) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(query, providerName, connectionString, parameters), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(DbCommand cmd, string columnName, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, closeWhenComplete), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(string query, string providerName, string connectionString, string columnName) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(query, providerName, connectionString), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(string query, string providerName, string connectionString, IEnumerable<DbParam> parameters, string columnName) where T : class, new()
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(query, providerName, connectionString, parameters), columnName);
        }

        public static int DbCommandTimeout { get; set; } = 180;
    }
}