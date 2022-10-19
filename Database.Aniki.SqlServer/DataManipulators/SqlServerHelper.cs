using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Database.Aniki.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Database.Aniki.DataManipulators
{
    internal static class SqlServerHelper
    {
        public static Result<List<string>> GetColumnToString(SqlCommand cmd, string connectionString, int columnIndex = 0)
        {
            var datatable = GetDataTable(cmd, connectionString);
            return new Result<List<string>>
            {
                Value = DataTableHelper.DataTableToListString(datatable.Value, columnIndex),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<string>> GetColumnToString(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return new Result<List<string>>
            {
                Value = DataTableHelper.DataTableToListString(datatable.Value, columnIndex),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<string>> GetColumnToString(SqlCommand cmd, string connectionString, string columnName)
        {
            var datatable = GetDataTable(cmd, connectionString);
            return new Result<List<string>>
            {
                Value = DataTableHelper.DataTableToListString(datatable.Value, columnName),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<string>> GetColumnToString(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return new Result<List<string>>
            {
                Value = DataTableHelper.DataTableToListString(datatable.Value, columnName),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetColumn<T>(SqlCommand cmd, string connectionString, int columnIndex = 0)
        {
            var datatable = GetDataTable(cmd, connectionString);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToListCast<T>(datatable.Value, columnIndex),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetColumn<T>(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToListCast<T>(datatable.Value, columnIndex),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetColumn<T>(SqlCommand cmd, string connectionString, string columnName)
        {
            var datatable = GetDataTable(cmd, connectionString);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToListCast<T>(datatable.Value, columnName),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetColumn<T>(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToListCast<T>(datatable.Value, columnName),
                ExecutionTime = datatable.ExecutionTime
            };
        }
        public static Result<DataSet> GetDataSet(SqlCommand cmd, string connectionString, string[] tableNames)
        {
            var dataSet = new DataSet();
            var result = new Result<DataSet>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
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
                sqlTransaction.Commit();
                var stats = sqlConnection.RetrieveStatistics();
                result.Value = dataSet;
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<DataSet> GetDataSet(SqlCommand cmd, SqlConnection connection, string[] tableNames, bool closeWhenComplete = false)
        {
            var dataSet = new DataSet();
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            var result = new Result<DataSet>();
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
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
                sqlTransaction.Commit();
                var stats = connection.RetrieveStatistics();
                result.Value = dataSet;
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static Result<DataSet> GetDataSet(string query, string connectionString, string[] tableNames)
        {
            using var sqlCommand = new SqlCommand(query);
            return GetDataSet(sqlCommand, connectionString, tableNames);
        }

        public static Result<DataSet> GetDataSet(string query, string connectionString, string[] tableNames, Array sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return GetDataSet(sqlCommand, connectionString, tableNames);
        }

        public static Result<DataSet> GetDataSet(string query, string connectionString, string[] tableNames, SqlParameter[] sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length != 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return GetDataSet(sqlCommand, connectionString, tableNames);
        }

        public static Result<DataTable> GetDataTable(SqlCommand cmd, string connectionString)
        {
            var dataTable = new DataTable();
            var result = new Result<DataTable>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                sqlConnection.Open();
                using var sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataTable);
                var stats = sqlConnection.RetrieveStatistics();
                result.Value = dataTable;
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<DataTable> GetDataTable(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
            var result = new Result<DataTable>();
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            using (var sqlTransaction = connection.BeginTransaction())
            {
                using var sqlDataAdapter = new SqlDataAdapter();
                cmd.Transaction = sqlTransaction;
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);
                sqlTransaction.Commit();
            }
            var stats = connection.RetrieveStatistics();
            result.Value = dataTable;
            result.ExecutionTime = (long)stats["ExecutionTime"];
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static Result<DataTable> GetDataTable(string query, string connectionString)
        {
            using var sqlCommand = new SqlCommand(query);
            return GetDataTable(sqlCommand, connectionString);
        }

        public static Result<DataTable> GetDataTable(string query, string connectionString, Array sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return GetDataTable(sqlCommand, connectionString);
        }

        public static Result<DataTable> GetDataTable(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length != 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return GetDataTable(sqlCommand, connectionString);
        }

        public static Result<List<T>> GetDataTable<T>(SqlCommand cmd, string connectionString) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connectionString);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToList<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetDataTable<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToList<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetDataTable<T>(string query, string connectionString) where T : class, new()
        {
            var datatable = GetDataTable(query, connectionString);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToList<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetDataTable<T>(string query, string connectionString, Array sqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, connectionString, sqlParameters);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToList<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<List<T>> GetDataTable<T>(string query, string connectionString, SqlParameter[] sqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, connectionString, sqlParameters);
            return new Result<List<T>>
            {
                Value = DataTableHelper.DataTableToList<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<T> GetDataRow<T>(SqlCommand cmd, string connectionString) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connectionString);
            return new Result<T>
            {
                Value = DataTableHelper.DataRowToT<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<T>? GetDataRow<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            var datatable = GetDataTable(cmd, connection, closeWhenComplete);
            return new Result<T>
            {
                Value = DataTableHelper.DataRowToT<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<T>? GetDataRow<T>(string query, string connectionString) where T : class, new()
        {
            var datatable = GetDataTable(query, connectionString);
            return new Result<T>
            {
                Value = DataTableHelper.DataRowToT<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<T>? GetDataRow<T>(string query, string connectionString, Array sqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, connectionString, sqlParameters);
            return new Result<T>
            {
                Value = DataTableHelper.DataRowToT<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<T>? GetDataRow<T>(string query, string connectionString, SqlParameter[] sqlParameters) where T : class, new()
        {
            var datatable = GetDataTable(query, connectionString, sqlParameters);
            return new Result<T>
            {
                Value = DataTableHelper.DataRowToT<T>(datatable.Value),
                ExecutionTime = datatable.ExecutionTime
            };
        }

        public static Result<Dictionary<T, U>>? GetDictionary<T, U>(string query, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            var result = new Result<Dictionary<T, U>> ();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[0], (U)sqlDataReader[1]);
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, U>>? GetDictionary<T, U>(SqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            var result = new Result<Dictionary<T, U>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[0], (U)sqlDataReader[1]);
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, U>>? GetDictionary<T, U>(string query, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            var result = new Result<Dictionary<T, U>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.FieldCount < 2 && 
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)((object)sqlDataReader[keyColumnIndex]), (U)((object)sqlDataReader[valueColumnIndex]));
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, U>>? GetDictionary<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            var result = new Result<Dictionary<T, U>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<string, string>>? GetDictionary(string query, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            var result = new Result<Dictionary<string, string>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<string, string>>? GetDictionary(SqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            var result = new Result<Dictionary<string, string>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<string, string>>? GetDictionary(string query, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            var result = new Result<Dictionary<string, string>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = SQLCommandTimeout;
                sqlCommand.CommandText = query;
                using var sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<string, string>>? GetDictionary(SqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            var result = new Result<Dictionary<string, string>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, U>>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            var result = new Result<Dictionary<T, U>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
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
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, U>>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            var result = new Result<Dictionary<T, U>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using SqlDataReader sqlDataReader = cmd.ExecuteReader();
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
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, List<U>>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            var result = new Result<Dictionary<T, List<U>>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
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
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<Dictionary<T, List<U>>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            var result = new Result<Dictionary<T, List<U>>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
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
                if (dictionary.Count == 0)
                    result.Value = null;
                else
                    result.Value = dictionary;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<List<List<string>>>? GetListListString(SqlCommand cmd, string connectionString)
        {
            var list = new List<List<string>>();
            var result = new Result<List<List<string>>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var list2 = new List<string>();
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        list2.Add(sqlDataReader[i].ToString());
                    }
                    list.Add(list2);
                }
                if (list.Count == 0)
                    result.Value = null;
                else
                    result.Value = list;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<List<List<string>>>? GetListListString(SqlCommand cmd, string connectionString, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            var result = new Result<List<List<string>>>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                using var sqlDataReader = cmd.ExecuteReader();
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
                if (list.Count == 0)
                    result.Value = null;
                else
                    result.Value = list;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<List<List<string>>>? GetListListString(string query, string connectionString)
        {
            using var sqlCommand = new SqlCommand(query);
            return GetListListString(sqlCommand, connectionString);
        }

        public static Result<List<List<string>>>? GetListListString(string query, string connectionString, string dateFormat)
        {
            using var sqlCommand = new SqlCommand(query);
            return GetListListString(sqlCommand, connectionString, dateFormat);
        }

        public static Result<object> GetScalar(SqlCommand cmd, string connectionString)
        {
            var result = new Result<object>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                var obj = cmd.ExecuteScalar();
                sqlTransaction.Commit();
                result.Value = obj;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static Result<object> GetScalar(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            var result = new Result<object>();
            using (var sqlTransaction = connection.BeginTransaction())
            {
                cmd.Transaction = sqlTransaction;
                result.Value = cmd.ExecuteScalar();
                sqlTransaction.Commit();
            }
            var stats = connection.RetrieveStatistics();
            result.ExecutionTime = (long)stats["ExecutionTime"];
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static Result<object> GetScalar(string query, string connectionString)
        {
            using var sqlCommand = new SqlCommand(query);
            return GetScalar(sqlCommand, connectionString);
        }

        public static Result<object> GetScalar(string query, string connectionString, Array sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return GetScalar(sqlCommand, connectionString);
        }

        public static Result<object> GetScalar(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length != 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return GetScalar(sqlCommand, connectionString);
        }

        public static StructResult<int> ExecuteNonQuery(SqlCommand cmd, string connectionString)
        {
            var result = new StructResult<int>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                int num = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                result.Value = num;
                var stats = sqlConnection.RetrieveStatistics();
                result.ExecutionTime = (long)stats["ExecutionTime"];
            }
            return result;
        }

        public static StructResult<int> ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            var result = new StructResult<int>();
            using (var sqlTransaction = connection.BeginTransaction())
            {
                cmd.Transaction = sqlTransaction;
                result.Value = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            var stats = connection.RetrieveStatistics();
            result.ExecutionTime = (long)stats["ExecutionTime"];
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static StructResult<int> ExecuteNonQuery(string query, string connectionString)
        {
            using SqlCommand sqlCommand = new SqlCommand(query);
            return ExecuteNonQuery(sqlCommand, connectionString);
        }

        public static StructResult<int> ExecuteNonQuery(string query, string connectionString, Array sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return ExecuteNonQuery(sqlCommand, connectionString);
        }
        public static StructResult<int> ExecuteNonQuery(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            using var sqlCommand = new SqlCommand(query);
            if (sqlParameters != null && sqlParameters.Length != 0)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            return ExecuteNonQuery(sqlCommand, connectionString);
        }


        public static int SQLCommandTimeout { get; set; } = 180;

    }
}
