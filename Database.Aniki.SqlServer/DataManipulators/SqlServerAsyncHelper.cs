using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Aniki.DataManipulators
{
    internal static class SqlServerAsyncHelper
    {
        public static async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connectionStringName), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connectionStringName), columnName);
        }

        public static async Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(SqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connectionStringName), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(SqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connectionStringName), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnName);
        }
        public static async Task<DataSet> GetDataSetAsync(SqlCommand cmd, string connectionString, string[] tableNames)
        {
            var dataSet = new DataSet();
            DataSet result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                using var sqlDataAdapter = new SqlDataAdapter();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                sqlDataAdapter.SelectCommand = cmd;
                for (int i = 0; i < tableNames.Length; i++)
                {
                    sqlDataAdapter.TableMappings.Add("Table" + ((i > 0) ? i.ToString() : ""), tableNames[i]);
                }
                sqlDataAdapter.Fill(dataSet);
                await sqlTransaction.CommitAsync();
                result = dataSet;
            }
            return result;
        }

        public static async Task<DataSet> GetDataSetAsync(SqlCommand cmd, SqlConnection connection, string[] tableNames, bool closeWhenComplete = false)
        {
            var dataSet = new DataSet();
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            using (var sqlTransaction = connection.BeginTransaction())
            {
                using var sqlDataAdapter = new SqlDataAdapter();
                cmd.Transaction = sqlTransaction;
                sqlDataAdapter.SelectCommand = cmd;
                for (int i = 0; i < tableNames.Length; i++)
                {
                    sqlDataAdapter.TableMappings.Add("Table" + ((i > 0) ? i.ToString() : ""), tableNames[i]);
                }
                sqlDataAdapter.Fill(dataSet);
                await sqlTransaction.CommitAsync();
            }
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return dataSet;
        }

        public static async Task<DataSet> GetDataSetAsync(string query, string connectionString, string[] tableNames)
        {
            DataSet dataSet;
            using (var sqlCommand = new SqlCommand(query))
            {
                dataSet = await GetDataSetAsync(sqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static async Task<DataSet> GetDataSetAsync(string query, string connectionString, string[] tableNames, Array sqlParameters)
        {
            DataSet dataSet;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataSet = await GetDataSetAsync(sqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static async Task<DataSet> GetDataSetAsync(string query, string connectionString, string[] tableNames, SqlParameter[] sqlParameters)
        {
            DataSet dataSet;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataSet = await GetDataSetAsync(sqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static async Task<DataTable> GetDataTableAsync(SqlCommand cmd, string connectionString)
        {
            var dataTable = new DataTable();
            DataTable result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                using var sqlDataAdapter = new SqlDataAdapter();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);
                await sqlTransaction.CommitAsync();
                result = dataTable;
            }
            return result;
        }

        public static async Task<DataTable> GetDataTableAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            using (var sqlTransaction = connection.BeginTransaction())
            {
                using var sqlDataAdapter = new SqlDataAdapter();
                cmd.Transaction = sqlTransaction;
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);
                await sqlTransaction.CommitAsync();
            }
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string connectionString)
        {
            DataTable dataTable;
            using (var sqlCommand = new SqlCommand(query))
            {
                dataTable = await GetDataTableAsync(sqlCommand, connectionString);
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string connectionString, Array sqlParameters)
        {
            DataTable dataTable;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataTable = await GetDataTableAsync(sqlCommand, connectionString);
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            DataTable dataTable;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataTable = await GetDataTableAsync(sqlCommand, connectionString);
            }
            return dataTable;
        }

        public static async Task<List<T>> GetDataTableAsync<T>(SqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(cmd, connectionString));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(cmd, connection, false));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, connectionString));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string connectionString, Array sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, connectionString, sqlParameters));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string connectionString, SqlParameter[] sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, connectionString, sqlParameters));
        }

        public static async Task<T?> GetDataRowAsync<T>(SqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(cmd, connectionString));
        }

        public static async Task<T?> GetDataRowAsync<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(cmd, connection, false));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, connectionString));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string connectionString, Array sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, connectionString, sqlParameters));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string connectionString, SqlParameter[] sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, connectionString, sqlParameters));
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[0], (U)sqlDataReader[1]);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(SqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[0], (U)sqlDataReader[1]);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2 && 
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<string, string>?> GetDictionaryAsync(string query, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<string, string>?> GetDictionaryAsync(SqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<string, string>?> GetDictionaryAsync(string query, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<string, string>?> GetDictionaryAsync(SqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = Shared._ObjectPropertiesCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!Shared._ObjectPropertiesCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        Shared._ObjectPropertiesCache.Add(typeFromHandle, list);
                    }
                }
                while (await sqlDataReader.ReadAsync())
                {
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], u);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(SqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = Shared._ObjectPropertiesCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!Shared._ObjectPropertiesCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        Shared._ObjectPropertiesCache.Add(typeFromHandle, list);
                    }
                }
                while (await sqlDataReader.ReadAsync())
                {
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    dictionary.Add((T)sqlDataReader[keyColumnName], u);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = Shared._ObjectPropertiesCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!Shared._ObjectPropertiesCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        Shared._ObjectPropertiesCache.Add(typeFromHandle, list);
                    }
                }
                while (await sqlDataReader.ReadAsync())
                {
                    bool flag = false;
                    if (dictionary.TryGetValue((T)sqlDataReader[keyColumnIndex], out List<U> list2))
                        flag = true;
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.Name]), null);
                            else
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                        }
                    }
                    if (flag)
                        list2.Add(u);
                    else
                    {
                        list2 = new List<U>
                        {
                            u
                        };
                        dictionary.Add((T)sqlDataReader[keyColumnIndex], list2);
                    }
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(SqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                var typeFromHandle = typeof(U);
                var objectPropertiesCache = Shared._ObjectPropertiesCache;
                List<PropertyInfo> list;
                lock (objectPropertiesCache)
                {
                    if (!Shared._ObjectPropertiesCache.TryGetValue(typeFromHandle, out list))
                    {
                        list = new List<PropertyInfo>(typeFromHandle.GetProperties());
                        list.RemoveAll((PropertyInfo item) => !item.CanWrite);
                        Shared._ObjectPropertiesCache.Add(typeFromHandle, list);
                    }
                }
                while (await sqlDataReader.ReadAsync())
                {
                    var flag = false;
                    if (dictionary.TryGetValue((T)((object)sqlDataReader[keyColumnName]), out List<U> list2))
                    {
                        flag = true;
                    }
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(sqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (flag)
                    {
                        list2.Add(u);
                    }
                    else
                    {
                        list2 = new List<U>
                        {
                            u
                        };
                        dictionary.Add((T)((object)sqlDataReader[keyColumnName]), list2);
                    }
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<List<List<string>>?> GetListListStringAsync(SqlCommand cmd, string connectionString)
        {
            var list = new List<List<string>>();
            List<List<string>>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    var list2 = new List<string>();
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        list2.Add(sqlDataReader[i].ToString());
                    }
                    list.Add(list2);
                }
                if (list.Count == 0)
                    result = null;
                else
                    result = list;
            }
            return result;
        }

        public static async Task<List<List<string>>?> GetListListStringAsync(SqlCommand cmd, string connectionString, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            List<List<string>>? result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = await cmd.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    List<string> list2 = new List<string>();
                    if (flag)
                    {
                        flag = false;
                        array = new bool[sqlDataReader.FieldCount];
                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {
                            if (sqlDataReader[i].GetType() == typeof(DateTime))
                            {
                                array[i] = true;
                                list2.Add(((DateTime)sqlDataReader[i]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(sqlDataReader[i].ToString());
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < sqlDataReader.FieldCount; j++)
                        {
                            if (array!=null && array[j])
                            {
                                list2.Add(((DateTime)sqlDataReader[j]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(sqlDataReader[j].ToString());
                            }
                        }
                    }
                    list.Add(list2);
                }
                if (list.Count == 0)
                    result = null;
                else
                    result = list;
            }
            return result;
        }

        public static async Task<List<List<string>>?> GetListListStringAsync(string query, string connectionString)
        {
            List<List<string>>? listListString;
            using (var sqlCommand = new SqlCommand(query))
            {
                listListString = await GetListListStringAsync(sqlCommand, connectionString);
            }
            return listListString;
        }

        public static async Task<List<List<string>>?> GetListListStringAsync(string query, string connectionString, string dateFormat)
        {
            List<List<string>>? listListString;
            using (var sqlCommand = new SqlCommand(query))
            {
                listListString = await GetListListStringAsync(sqlCommand, connectionString, dateFormat);
            }
            return listListString;
        }

        public static async Task<object> GetScalarAsync(SqlCommand cmd, string connectionString)
        {
            object result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                var obj = await cmd.ExecuteScalarAsync();
                await sqlTransaction.CommitAsync();
                result = obj;
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            object result;
            using (var sqlTransaction = connection.BeginTransaction())
            {
                cmd.Transaction = sqlTransaction;
                result = await cmd.ExecuteScalarAsync();
                sqlTransaction.Commit();
            }
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(string query, string connectionString)
        {
            object scalar;
            using (var sqlCommand = new SqlCommand(query))
            {
                scalar = await GetScalarAsync(sqlCommand, connectionString);
            }
            return scalar;
        }

        public static async Task<object> GetScalarAsync(string query, string connectionString, Array sqlParameters)
        {
            object scalar;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                scalar = await GetScalarAsync(sqlCommand, connectionString);
            }
            return scalar;
        }

        public static async Task<object> GetScalarAsync(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            object scalar;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                scalar = await GetScalarAsync(sqlCommand, connectionString);
            }
            return scalar;
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlCommand cmd, string connectionString)
        {
            int result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                int num = await cmd.ExecuteNonQueryAsync();
                await sqlTransaction.CommitAsync();
                result = num;
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            int result;
            using (var sqlTransaction = connection.BeginTransaction())
            {
                cmd.Transaction = sqlTransaction;
                result = await cmd.ExecuteNonQueryAsync();
                await sqlTransaction.CommitAsync();
            }
            if (closeWhenComplete)
            {
                await connection.CloseAsync();
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, string connectionString)
        {
            int result;
            using (var sqlCommand = new SqlCommand(query))
            {
                result = await ExecuteNonQueryAsync(sqlCommand, connectionString);
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, string connectionString, Array sqlParameters)
        {
            int result;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                result = await ExecuteNonQueryAsync(sqlCommand, connectionString);
            }
            return result;
        }
        public static async Task<int> ExecuteNonQueryAsync(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            int result;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                result = await ExecuteNonQueryAsync(sqlCommand, connectionString);
            }
            return result;
        }


        public static int SQLCommandTimeout { get; set; } = 180;
    }
}
