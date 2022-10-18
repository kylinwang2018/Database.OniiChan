using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Aniki.PostgreSQL.DataManipulators
{
    internal class NpgServerAsyncHelper
    {
        public static async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connectionStringName), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connectionStringName), columnName);
        }

        public static async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(NpgsqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connectionStringName), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static async Task<List<T>> GetColumnAsync<T>(NpgsqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connectionStringName), columnName);
        }

        public static async Task<List<T>> GetColumnAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(await GetDataTableAsync(cmd, connection, closeWhenComplete), columnName);
        }
        public static async Task<DataSet> GetDataSetAsync(NpgsqlCommand cmd, string connectionString, string[] tableNames)
        {
            var dataSet = new DataSet();
            DataSet result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                using var NpgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                NpgsqlDataAdapter.SelectCommand = cmd;
                for (int i = 0; i < tableNames.Length; i++)
                {
                    NpgsqlDataAdapter.TableMappings.Add("Table" + ((i > 0) ? i.ToString() : ""), tableNames[i]);
                }
                NpgsqlDataAdapter.Fill(dataSet);
                await sqlTransaction.CommitAsync();
                result = dataSet;
            }
            return result;
        }

        public static async Task<DataSet> GetDataSetAsync(NpgsqlCommand cmd, NpgsqlConnection connection, string[] tableNames, bool closeWhenComplete = false)
        {
            var dataSet = new DataSet();
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            using (var sqlTransaction = connection.BeginTransaction())
            {
                using var NpgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.Transaction = sqlTransaction;
                NpgsqlDataAdapter.SelectCommand = cmd;
                for (int i = 0; i < tableNames.Length; i++)
                {
                    NpgsqlDataAdapter.TableMappings.Add("Table" + ((i > 0) ? i.ToString() : ""), tableNames[i]);
                }
                NpgsqlDataAdapter.Fill(dataSet);
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
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                dataSet = await GetDataSetAsync(NpgsqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static async Task<DataSet> GetDataSetAsync(string query, string connectionString, string[] tableNames, Array NpgsqlParameters)
        {
            DataSet dataSet;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataSet = await GetDataSetAsync(NpgsqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static async Task<DataSet> GetDataSetAsync(string query, string connectionString, string[] tableNames, NpgsqlParameter[] NpgsqlParameters)
        {
            DataSet dataSet;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataSet = await GetDataSetAsync(NpgsqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static async Task<DataTable> GetDataTableAsync(NpgsqlCommand cmd, string connectionString)
        {
            var dataTable = new DataTable();
            DataTable result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                using var NpgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                NpgsqlDataAdapter.SelectCommand = cmd;
                NpgsqlDataAdapter.Fill(dataTable);
                await sqlTransaction.CommitAsync();
                result = dataTable;
            }
            return result;
        }

        public static async Task<DataTable> GetDataTableAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.CloseAsync();
                await connection.OpenAsync();
            }
            using (var sqlTransaction = connection.BeginTransaction())
            {
                using var NpgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.Transaction = sqlTransaction;
                NpgsqlDataAdapter.SelectCommand = cmd;
                NpgsqlDataAdapter.Fill(dataTable);
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
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                dataTable = await GetDataTableAsync(NpgsqlCommand, connectionString);
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string connectionString, Array NpgsqlParameters)
        {
            DataTable dataTable;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataTable = await GetDataTableAsync(NpgsqlCommand, connectionString);
            }
            return dataTable;
        }

        public static async Task<DataTable> GetDataTableAsync(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters)
        {
            DataTable dataTable;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataTable = await GetDataTableAsync(NpgsqlCommand, connectionString);
            }
            return dataTable;
        }

        public static async Task<List<T>> GetDataTableAsync<T>(NpgsqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(cmd, connectionString));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(cmd, connection, false));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, connectionString));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string connectionString, Array NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, connectionString, NpgsqlParameters));
        }

        public static async Task<List<T>> GetDataTableAsync<T>(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(await GetDataTableAsync(query, connectionString, NpgsqlParameters));
        }

        public static async Task<T?> GetDataRowAsync<T>(NpgsqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(cmd, connectionString));
        }

        public static async Task<T?> GetDataRowAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(cmd, connection, false));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, connectionString));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string connectionString, Array NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, connectionString, NpgsqlParameters));
        }

        public static async Task<T?> GetDataRowAsync<T>(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(await GetDataTableAsync(query, connectionString, NpgsqlParameters));
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = await NpgsqlCommand.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)NpgsqlDataReader[0], (U)NpgsqlDataReader[1]);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(NpgsqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (NpgsqlConnection NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using NpgsqlDataReader NpgsqlDataReader = await cmd.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)NpgsqlDataReader[0], (U)NpgsqlDataReader[1]);
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
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = await NpgsqlCommand.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)NpgsqlDataReader[keyColumnIndex], (U)NpgsqlDataReader[valueColumnIndex]);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(NpgsqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)NpgsqlDataReader[keyColumnIndex], (U)NpgsqlDataReader[valueColumnIndex]);
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
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = await NpgsqlCommand.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add(NpgsqlDataReader[0].ToString(), NpgsqlDataReader[1].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<string, string>?> GetDictionaryAsync(NpgsqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add(NpgsqlDataReader[0].ToString(), NpgsqlDataReader[1].ToString());
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
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = await NpgsqlCommand.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add(NpgsqlDataReader[keyColumnIndex].ToString(), NpgsqlDataReader[valueColumnIndex].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<string, string>?> GetDictionaryAsync(NpgsqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using NpgsqlDataReader NpgsqlDataReader = await cmd.ExecuteReaderAsync();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await NpgsqlDataReader.ReadAsync())
                {
                    dictionary.Add(NpgsqlDataReader[keyColumnIndex].ToString(), NpgsqlDataReader[valueColumnIndex].ToString());
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(NpgsqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
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
                while (await NpgsqlDataReader.ReadAsync())
                {
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)NpgsqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(NpgsqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    dictionary.Add((T)NpgsqlDataReader[keyColumnIndex], u);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(NpgsqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using NpgsqlDataReader NpgsqlDataReader = await cmd.ExecuteReaderAsync();
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
                while (await NpgsqlDataReader.ReadAsync())
                {
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                            {
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)NpgsqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(NpgsqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    dictionary.Add((T)NpgsqlDataReader[keyColumnName], u);
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(NpgsqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
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
                while (await NpgsqlDataReader.ReadAsync())
                {
                    bool flag = false;
                    if (dictionary.TryGetValue((T)NpgsqlDataReader[keyColumnIndex], out List<U> list2))
                        flag = true;
                    U u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in list)
                    {
                        try
                        {
                            bool isEnum = propertyInfo.PropertyType.IsEnum;
                            if (isEnum)
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)NpgsqlDataReader[propertyInfo.Name]), null);
                            else
                                propertyInfo.SetValue(u, Convert.ChangeType(NpgsqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
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
                        dictionary.Add((T)NpgsqlDataReader[keyColumnIndex], list2);
                    }
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(NpgsqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
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
                while (await NpgsqlDataReader.ReadAsync())
                {
                    var flag = false;
                    if (dictionary.TryGetValue((T)((object)NpgsqlDataReader[keyColumnName]), out List<U> list2))
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
                                propertyInfo.SetValue(u, Enum.ToObject(propertyInfo.PropertyType, (int)NpgsqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(u, Convert.ChangeType(NpgsqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
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
                        dictionary.Add((T)((object)NpgsqlDataReader[keyColumnName]), list2);
                    }
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static async Task<List<List<string>>?> GetListListStringAsync(NpgsqlCommand cmd, string connectionString)
        {
            var list = new List<List<string>>();
            List<List<string>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
                while (await NpgsqlDataReader.ReadAsync())
                {
                    var list2 = new List<string>();
                    for (int i = 0; i < NpgsqlDataReader.FieldCount; i++)
                    {
                        list2.Add(NpgsqlDataReader[i].ToString());
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

        public static async Task<List<List<string>>?> GetListListStringAsync(NpgsqlCommand cmd, string connectionString, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            List<List<string>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = await cmd.ExecuteReaderAsync();
                while (await NpgsqlDataReader.ReadAsync())
                {
                    List<string> list2 = new List<string>();
                    if (flag)
                    {
                        flag = false;
                        array = new bool[NpgsqlDataReader.FieldCount];
                        for (int i = 0; i < NpgsqlDataReader.FieldCount; i++)
                        {
                            if (NpgsqlDataReader[i].GetType() == typeof(DateTime))
                            {
                                array[i] = true;
                                list2.Add(((DateTime)NpgsqlDataReader[i]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(NpgsqlDataReader[i].ToString());
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < NpgsqlDataReader.FieldCount; j++)
                        {
                            if (array != null && array[j])
                            {
                                list2.Add(((DateTime)NpgsqlDataReader[j]).ToString(dateFormat));
                            }
                            else
                            {
                                list2.Add(NpgsqlDataReader[j].ToString());
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
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                listListString = await GetListListStringAsync(NpgsqlCommand, connectionString);
            }
            return listListString;
        }

        public static async Task<List<List<string>>?> GetListListStringAsync(string query, string connectionString, string dateFormat)
        {
            List<List<string>>? listListString;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                listListString = await GetListListStringAsync(NpgsqlCommand, connectionString, dateFormat);
            }
            return listListString;
        }

        public static async Task<object> GetScalarAsync(NpgsqlCommand cmd, string connectionString)
        {
            object result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                var obj = await cmd.ExecuteScalarAsync();
                await sqlTransaction.CommitAsync();
                result = obj;
            }
            return result;
        }

        public static async Task<object> GetScalarAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
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
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                scalar = await GetScalarAsync(NpgsqlCommand, connectionString);
            }
            return scalar;
        }

        public static async Task<object> GetScalarAsync(string query, string connectionString, Array NpgsqlParameters)
        {
            object scalar;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                scalar = await GetScalarAsync(NpgsqlCommand, connectionString);
            }
            return scalar;
        }

        public static async Task<object> GetScalarAsync(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters)
        {
            object scalar;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                scalar = await GetScalarAsync(NpgsqlCommand, connectionString);
            }
            return scalar;
        }

        public static async Task<int> ExecuteNonQueryAsync(NpgsqlCommand cmd, string connectionString)
        {
            int result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await NpgsqlConnection.OpenAsync();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                int num = await cmd.ExecuteNonQueryAsync();
                await sqlTransaction.CommitAsync();
                result = num;
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
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
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                result = await ExecuteNonQueryAsync(NpgsqlCommand, connectionString);
            }
            return result;
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, string connectionString, Array NpgsqlParameters)
        {
            int result;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                result = await ExecuteNonQueryAsync(NpgsqlCommand, connectionString);
            }
            return result;
        }
        public static async Task<int> ExecuteNonQueryAsync(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters)
        {
            int result;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                result = await ExecuteNonQueryAsync(NpgsqlCommand, connectionString);
            }
            return result;
        }


        public static int NpgsqlCommandTimeout { get; set; } = 180;
    }
}
