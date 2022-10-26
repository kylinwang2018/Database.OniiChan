﻿using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Database.Aniki.PostgreSQL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Database.Aniki
{
    internal partial class NpgsqlDbContext : INpgsqlDbContext
    {
        #region GetColumnToString
        public List<string> GetColumnToString(NpgsqlCommand cmd, int columnIndex = 0)
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public List<string> GetColumnToString(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnIndex);
        }

        public List<string> GetColumnToString(NpgsqlCommand cmd, string columnName)
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }

        public List<string> GetColumnToString(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToListString(datatable, columnName);
        }
        #endregion

        #region GetDataTable
        public DataTable GetDataTable(NpgsqlCommand cmd)
        {
            var dataTable = new DataTable();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                cmd.Connection = NpgsqlConnection;
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
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

        public DataTable GetDataTable(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            try
            {
                var dataTable = new DataTable();
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    using var sqlDataAdapter = _connectionFactory.CreateDataAdapter();
                    cmd.Transaction = sqlTransaction;
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(dataTable);
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public DataTable GetDataTable(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetDataTable(NpgsqlCommand);
        }

        public DataTable GetDataTable(string query, CommandType commandType, Array NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return GetDataTable(NpgsqlCommand);
        }

        public DataTable GetDataTable(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return GetDataTable(NpgsqlCommand);
        }

        public List<T> GetDataTable<T>(NpgsqlCommand cmd) where T : class, new()
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataTableToList<T>(datatable);
        }

        public List<T> GetDataTable<T>(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataTableToList<T>(datatable);
        }
        #endregion

        #region GetDataRow
        public T? GetDataRow<T>(NpgsqlCommand cmd) where T : class, new()
        {
            var datatable = GetDataTable(cmd);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(string query, CommandType commandType) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataRowToT<T>(datatable);
        }

        public T? GetDataRow<T>(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, commandType, NpgsqlParameters);
            return DataTableHelper.DataRowToT<T>(datatable);
        }
        #endregion

        #region GetDictionary
        public Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                using var NpgsqlCommand = NpgsqlConnection.CreateCommand();
                NpgsqlCommand.CommandType = commandType;
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var sqlDataReader = NpgsqlCommand.ExecuteReaderWithRetry(_sqlRetryOption);;
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[0], (U)sqlDataReader[1]);
                }
                LogSqlInfo(NpgsqlCommand, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<T, U>? GetDictionary<T, U>(NpgsqlCommand cmd)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[0], (U)sqlDataReader[1]);
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

        public Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();

            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                using var NpgsqlCommand = _connectionFactory.CreateCommand();
                NpgsqlCommand.CommandType = commandType;
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
                
                NpgsqlCommand.CommandText = query;
                using var sqlDataReader = NpgsqlCommand.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)((object)sqlDataReader[keyColumnIndex]), (U)((object)sqlDataReader[valueColumnIndex]));
                }
                LogSqlInfo(NpgsqlCommand, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<T, U>? GetDictionary<T, U>(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
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

        public Dictionary<string, string>? GetDictionary(string query, CommandType commandType)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using NpgsqlConnection NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                using var NpgsqlCommand = _connectionFactory.CreateCommand();
                NpgsqlCommand.CommandType = commandType;
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
                
                NpgsqlCommand.CommandText = query;
                using var sqlDataReader = NpgsqlCommand.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());
                }
                LogSqlInfo(NpgsqlCommand, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<string, string>? GetDictionary(NpgsqlCommand cmd)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());
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

        public Dictionary<string, string>? GetDictionary(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                using var NpgsqlCommand = _connectionFactory.CreateCommand();
                NpgsqlCommand.CommandType = commandType;
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
                
                NpgsqlCommand.CommandText = query;
                using var sqlDataReader = NpgsqlCommand.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                LogSqlInfo(NpgsqlCommand, NpgsqlConnection);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                throw;
            }

            if (dictionary.Any())
                return null;
            else
                return dictionary;
        }

        public Dictionary<string, string>? GetDictionary(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
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
        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
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
                while (sqlDataReader.Read())
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

        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
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
                while (sqlDataReader.Read())
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

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
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
                while (sqlDataReader.Read())
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

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
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
                while (sqlDataReader.Read())
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
        public List<List<string>>? GetListListString(NpgsqlCommand cmd)
        {
            var list = new List<List<string>>();
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                while (sqlDataReader.Read())
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

        public List<List<string>>? GetListListString(NpgsqlCommand cmd, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                cmd.Connection = NpgsqlConnection;
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                while (sqlDataReader.Read())
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

        public List<List<string>>? GetListListString(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetListListString(NpgsqlCommand);
        }

        public List<List<string>>? GetListListString(string query, CommandType commandType, string dateFormat)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetListListString(NpgsqlCommand, dateFormat);
        }
        #endregion

        #region GetListOf<T>
        public List<T>? GetListOf<T>(NpgsqlCommand cmd, NpgsqlConnection connection)
        {
            var list = new List<T>();
            var type = typeof(T);
            cmd.Connection = connection;
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                using var sqlDataReader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                while (sqlDataReader.Read())
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

        public List<T>? GetListOf<T>(NpgsqlCommand cmd)
        {
            using var NpgsqlConnection = _connectionFactory.CreateConnection();
            NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
            cmd.Connection = NpgsqlConnection;
            return GetListOf<T>(cmd,NpgsqlConnection);
        }

        public List<T>? GetListOf<T>(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return GetListOf<T>(NpgsqlCommand);
        }

        public List<T>? GetListOf<T>(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return GetListOf<T>(NpgsqlCommand);
        }

        #endregion

        #region GetScalar
        public object? GetScalar(NpgsqlCommand cmd)
        {
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.Transaction = sqlTransaction;
                var obj = cmd.ExecuteScalarWithRetry(_sqlRetryOption);
                sqlTransaction.Commit();
                LogSqlInfo(cmd, NpgsqlConnection);
                return obj;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
        }

        public object GetScalar(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            object result;
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = sqlTransaction;
                    result = cmd.ExecuteScalarWithRetry(_sqlRetryOption);
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public object? GetScalar(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            
            return GetScalar(NpgsqlCommand);
        }

        public object? GetScalar(string query, CommandType commandType, Array NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return GetScalar(NpgsqlCommand);
        }

        public object? GetScalar(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return GetScalar(NpgsqlCommand);
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(NpgsqlCommand cmd)
        {
            int result;
            try
            {
                using var NpgsqlConnection = _connectionFactory.CreateConnection();
                NpgsqlConnection.OpenWithRetry(_sqlRetryOption);
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.Transaction = sqlTransaction;
                int num = cmd.ExecuteNonQueryWithRetry(_sqlRetryOption);
                sqlTransaction.Commit();
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

        public int ExecuteNonQuery(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            int result;
            try
            {
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                using (var sqlTransaction = connection.BeginTransaction())
                {
                    cmd.Transaction = sqlTransaction;
                    result = cmd.ExecuteNonQueryWithRetry(_sqlRetryOption);
                    sqlTransaction.Commit();
                }
                LogSqlInfo(cmd, connection);
                if (closeWhenComplete)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                }
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                throw;
            }
            return result;
        }

        public int ExecuteNonQuery(string query, CommandType commandType)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            return ExecuteNonQuery(NpgsqlCommand);
        }

        public int ExecuteNonQuery(string query, CommandType commandType, Array NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return ExecuteNonQuery(NpgsqlCommand);
        }
        public int ExecuteNonQuery(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters)
        {
            using var NpgsqlCommand = _connectionFactory.CreateCommand();
            NpgsqlCommand.CommandText = query;
            NpgsqlCommand.CommandType = commandType;
            NpgsqlCommand.CommandTimeout = _options.DbCommandTimeout;
            NpgsqlCommand.AttachParameters(NpgsqlParameters);
            return ExecuteNonQuery(NpgsqlCommand);
        }
        #endregion

        #region ExecuteReader
        public IDataReader ExecuteReader(string query, CommandType commandType)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.OpenWithRetry(_sqlRetryOption);
                var cmd = _connectionFactory.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;
                var reader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch(Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReader(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.OpenWithRetry(_sqlRetryOption);
                return ExecuteReader(connection, null, commandType, query, NpgsqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReader(NpgsqlCommand cmd)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.OpenWithRetry(_sqlRetryOption);
                var reader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReader(NpgsqlCommand cmd, NpgsqlConnection connection)
        {
            try
            {
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                var reader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReader(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, NpgsqlParameter[] commandParameters)
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
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                // Create a reader
                var reader = cmd.ExecuteReaderWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }
        #endregion

        #region ExecuteReaderSequential
        public IDataReader ExecuteReaderSequential(string query, CommandType commandType)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.OpenWithRetry(_sqlRetryOption);
                var cmd = _connectionFactory.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _options.DbCommandTimeout;

                var reader = cmd.ExecuteReaderSequentialWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReaderSequential(string query, CommandType commandType, NpgsqlParameter[] NpgsqlParameters)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.OpenWithRetry(_sqlRetryOption);
                return ExecuteReader(connection, null, commandType, query, NpgsqlParameters);
            }
            catch (Exception ex)
            {
                LogSqlError(query, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReaderSequential(NpgsqlCommand cmd)
        {
            NpgsqlConnection? connection = null;
            try
            {
                connection = _connectionFactory.CreateConnection();
                connection.OpenWithRetry(_sqlRetryOption);
                var reader = cmd.ExecuteReaderSequentialWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReaderSequential(NpgsqlCommand cmd, NpgsqlConnection connection)
        {
            try
            {
                cmd.Connection = connection;
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                {
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }
                var reader = cmd.ExecuteReaderSequentialWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(cmd, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }

        public IDataReader ExecuteReaderSequential(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, NpgsqlParameter[] commandParameters)
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
                    connection.CloseWithRetry(_sqlRetryOption);
                    connection.OpenWithRetry(_sqlRetryOption);
                }

                // Create a reader
                var reader = cmd.ExecuteReaderSequentialWithRetry(_sqlRetryOption);
                LogSqlInfo(cmd, connection);
                return reader;
            }
            catch (Exception ex)
            {
                LogSqlError(commandText, ex);
                connection?.CloseWithRetry(_sqlRetryOption);
                throw;
            }
        }
        #endregion
    }
}