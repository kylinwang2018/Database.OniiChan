using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Database.Aniki.PostgreSQL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Database.Aniki
{
    internal partial class NpgsqlDbContext<T> : INpgsqlDbContext<T> where T : class, IDbContextOptions
    {
        #region GetColumnToString
        public async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, int columnIndex = 0)
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, string columnName)
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }

        public async Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }
        #endregion

        #region GetDataTable
        public async Task<DataTable> GetDataTableAsync(NpgsqlCommand cmd)
        {
            var dataTable = new DataTable();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                cmd.Connection = NpgsqlConnection;
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return dataTable;
        }

        public async Task<DataTable> GetDataTableAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            try
            {
                var dataTable = new DataTable();
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }
                using (var NpgsqlTransaction = connection.BeginTransaction())
                {
                    using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                    cmd.Transaction = NpgsqlTransaction;
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataTable);
                    await NpgsqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public async Task<DataTable> GetDataTableAsync(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            return await GetDataTableAsync(NpgsqlCommand);
        }

        public async Task<DataTable> GetDataTableAsync(string query, CommandType commandType, Array NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await GetDataTableAsync(NpgsqlCommand);
        }

        public async Task<DataTable> GetDataTableAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await GetDataTableAsync(NpgsqlCommand);
        }

        public async Task<List<T>> GetDataTableAsync<T>(NpgsqlCommand cmd) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public async Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataTableToList<T>(datatable);
        }
        #endregion

        #region GetDataRow
        public async Task<T?> GetDataRowAsync<T>(NpgsqlCommand cmd) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T?> GetDataRowAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = await GetDataTableAsync(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T?> GetDataRowAsync<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T?> GetDataRowAsync<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public async Task<T?> GetDataRowAsync<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            var datatable = await GetDataTableAsync(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataRowToT<T>(datatable);
        }
        #endregion

        #region GetDictionary
        public async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType)
        {
            return await GetDictionaryAsync<T, U>(query, commandType, 0, 1);
        }
        public async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType, params NpgsqlParameter[] parameters)
        {
            return await GetDictionaryAsync<T, U>(query, commandType, 0, 1, parameters);
        }

        public async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.CommandText = query;
            return await GetDictionaryAsync<T, U>(NpgsqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex, params NpgsqlParameter[] parameters)
        {
            var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.AttachParameters(parameters);
            return await GetDictionaryAsync<T, U>(NpgsqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(NpgsqlCommand cmd)
        {
            return await GetDictionaryAsync<T, U>(cmd, 0, 1);
        }

        public async Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public async Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType)
        {
            return await GetDictionaryAsync(query, commandType, 0, 1);
        }

        public async Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType, params NpgsqlParameter[] parameters)
        {
            return await GetDictionaryAsync(query, commandType, 0, 1, parameters);
        }

        public async Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.CommandText = query;
            return await GetDictionaryAsync(NpgsqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex, params NpgsqlParameter[] parameters)
        {
            var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.AttachParameters(parameters);
            return await GetDictionaryAsync(NpgsqlCommand, keyColumnIndex, valueColumnIndex);
        }

        public async Task<Dictionary<string, string>?> GetDictionaryAsync(NpgsqlCommand cmd)
        {
            return await GetDictionaryAsync(cmd, 0, 1);
        }

        public async Task<Dictionary<string, string>?> GetDictionaryAsync(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }
        #endregion

        #region GetDictionaryOfObjects
        public async Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
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
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public async Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
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
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public async Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
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
                    if (dictionary.TryGetValue((T)((object)sqlDataReader[keyColumnIndex]), out List<U> list2))
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
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public async Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
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
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }
        #endregion

        #region GetListListString
        public async Task<List<List<string>>?> GetListListStringAsync(NpgsqlCommand cmd)
        {
            var list = new List<List<string>>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                while (await sqlDataReader.ReadAsync())
                {
                    var list2 = new List<string>();
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        list2.Add(sqlDataReader[i].ToString());
                    }
                    list.Add(list2);
                }
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (list.Any())
                return null;
            else
                return list;
        }

        public async Task<List<List<string>>?> GetListListStringAsync(NpgsqlCommand cmd, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
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
                            if (array[j])
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
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            if (list.Any())
                return null;
            else
                return list;
        }

        public async Task<List<List<string>>?> GetListListStringAsync(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetListListStringAsync(NpgsqlCommand);
        }

        public async Task<List<List<string>>?> GetListListStringAsync(string query, CommandType commandType, string dateFormat)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;

            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetListListStringAsync(NpgsqlCommand, dateFormat);
        }
        #endregion

        #region GetListOf<T>
        public async Task<List<T>> GetListOfAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection)
        {
            var list = new List<T>();
            var type = typeof(T);
            cmd.Connection = connection;

            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }
                using var sqlDataReader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                while (await sqlDataReader.ReadAsync())
                {
                    T obj = (T)Activator.CreateInstance(type);
                    foreach (var propertyInfo in type.GetProperties())
                    {
                        try
                        {
                            if (propertyInfo.PropertyType.IsEnum)
                            {
                                propertyInfo.SetValue(obj, Enum.ToObject(propertyInfo.PropertyType, (int)sqlDataReader[propertyInfo.Name]), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, Convert.ChangeType(sqlDataReader[propertyInfo.Name], Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    list.Add(obj);
                }
                LogSqlInfo(cmd, connection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return list;
        }

        public async Task<List<T>> GetListOfAsync<T>(NpgsqlCommand cmd)
        {
            using var NpgsqlConnection = _connectionFactory.CreateConnection();
            await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
            cmd.Connection = NpgsqlConnection;
            return await GetListOfAsync<T>(cmd, NpgsqlConnection);
        }

        public async Task<List<T>> GetListOfAsync<T>(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;

            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return await GetListOfAsync<T>(NpgsqlCommand);
        }

        public async Task<List<T>> GetListOfAsync<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;

            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await GetListOfAsync<T>(NpgsqlCommand);
        }

        #endregion

        #region GetScalar
        public async Task<object> GetScalarAsync(NpgsqlCommand cmd)
        {
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                using var NpgsqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.Transaction = NpgsqlTransaction;
                var obj = await cmd.ExecuteScalarWithRetryAsync(_sqlRetryOption);
                await NpgsqlTransaction.CommitAsync();
                LogSqlInfo(cmd, NpgsqlConnection);
                return obj;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public async Task<object> GetScalarAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            object result;
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }
                using (var NpgsqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = NpgsqlTransaction;
                    result = await cmd.ExecuteScalarWithRetryAsync(_sqlRetryOption);
                    await NpgsqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public async Task<object> GetScalarAsync(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            return await GetScalarAsync(NpgsqlCommand);
        }

        public async Task<object> GetScalarAsync(string query, CommandType commandType, Array NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await GetScalarAsync(NpgsqlCommand);
        }

        public async Task<object> GetScalarAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await GetScalarAsync(NpgsqlCommand);
        }
        #endregion

        #region ExecuteNonQuery
        public async Task<int> ExecuteNonQueryAsync(NpgsqlCommand cmd)
        {
            int result;
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                await NpgsqlConnection.OpenWithRetryAsync(_sqlRetryOption);
                using var NpgsqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.Transaction = NpgsqlTransaction;
                int num = await cmd.ExecuteNonQueryWithRetryAsync(_sqlRetryOption);
                await NpgsqlTransaction.CommitAsync();
                result = num;
                LogSqlInfo(cmd, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }

            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            int result;
            try
            {
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }
                using (var NpgsqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = NpgsqlTransaction;
                    result = await cmd.ExecuteNonQueryWithRetryAsync(_sqlRetryOption);
                    await NpgsqlTransaction.CommitAsync();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    await connection.CloseWithRetryAsync(_sqlRetryOption);
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;

            return await ExecuteNonQueryAsync(NpgsqlCommand);
        }

        public async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, Array NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await ExecuteNonQueryAsync(NpgsqlCommand);
        }
        public async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return await ExecuteNonQueryAsync(NpgsqlCommand);
        }
        #endregion

        #region ExecuteReader
        public async Task<NpgsqlDataReader> ExecuteReaderAsync(string query, CommandType commandType)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                await connection.OpenWithRetryAsync(_sqlRetryOption);
                var cmd = _connectionFactory.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;

                var reader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                await connection.OpenWithRetryAsync(_sqlRetryOption);

                return await ExecuteReaderAsync(connection, null, commandType, query, NpgsqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlCommand cmd)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                await connection.OpenWithRetryAsync(_sqlRetryOption);
                var reader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlCommand cmd, NpgsqlConnection connection)
        {
            try
            {
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }
                var reader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            var cmd = _connectionFactory.CreateCommand();

            cmd.CommandTimeout = _options.DbCommandTimeout;

            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.Connection = connection;
            cmd.AttachParameters(commandParameters);
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
                cmd.Transaction = transaction;
            }

            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }

                // Create a reader
                var reader = await cmd.ExecuteReaderWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }
        #endregion

        #region ExecuteReaderSequential
        public async Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(string query, CommandType commandType)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                await connection.OpenWithRetryAsync(_sqlRetryOption);
                var cmd = _connectionFactory.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;

                var reader = await cmd.ExecuteReaderSequentialWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                await connection.OpenWithRetryAsync(_sqlRetryOption);

                return await ExecuteReaderSequentialAsync(connection, null, commandType, query, NpgsqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(NpgsqlCommand cmd)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                await connection.OpenWithRetryAsync(_sqlRetryOption);
                var reader = await cmd.ExecuteReaderSequentialWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(NpgsqlCommand cmd, NpgsqlConnection connection)
        {
            try
            {
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }
                var reader = await cmd.ExecuteReaderSequentialWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }

        public async Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            var cmd = _connectionFactory.CreateCommand();

            cmd.CommandTimeout = _options.DbCommandTimeout;

            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.Connection = connection;
            cmd.AttachParameters(commandParameters);
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
                cmd.Transaction = transaction;
            }

            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                    await connection.OpenWithRetryAsync(_sqlRetryOption);
                }

                // Create a reader
                var reader = await cmd.ExecuteReaderSequentialWithRetryAsync(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                connection?.CloseWithRetryAsync(_sqlRetryOption);
                throw;
            }
        }
        #endregion
    }
}
