using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Database.Aniki.PostgreSQL.DataManipulators
{
    internal class NpgServerHelper
    {
        public static List<string> GetColumnToString(NpgsqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connectionStringName), columnIndex);
        }

        public static List<string> GetColumnToString(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static List<string> GetColumnToString(NpgsqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connectionStringName), columnName);
        }

        public static List<string> GetColumnToString(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connection, closeWhenComplete), columnName);
        }

        public static List<T> GetColumn<T>(NpgsqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connectionStringName), columnIndex);
        }

        public static List<T> GetColumn<T>(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static List<T> GetColumn<T>(NpgsqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connectionStringName), columnName);
        }

        public static List<T> GetColumn<T>(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connection, closeWhenComplete), columnName);
        }
        public static DataSet GetDataSet(NpgsqlCommand cmd, string connectionString, string[] tableNames)
        {
            var dataSet = new DataSet();
            DataSet result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
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
                sqlTransaction.Commit();
                result = dataSet;
            }
            return result;
        }

        public static DataSet GetDataSet(NpgsqlCommand cmd, NpgsqlConnection connection, string[] tableNames, bool closeWhenComplete = false)
        {
            var dataSet = new DataSet();
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
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
                sqlTransaction.Commit();
            }
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return dataSet;
        }

        public static DataSet GetDataSet(string query, string connectionString, string[] tableNames)
        {
            DataSet dataSet;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                dataSet = GetDataSet(NpgsqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static DataSet GetDataSet(string query, string connectionString, string[] tableNames, Array NpgsqlParameters)
        {
            DataSet dataSet;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataSet = GetDataSet(NpgsqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static DataSet GetDataSet(string query, string connectionString, string[] tableNames, NpgsqlParameter[] NpgsqlParameters)
        {
            DataSet dataSet;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataSet = GetDataSet(NpgsqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static DataTable GetDataTable(NpgsqlCommand cmd, string connectionString)
        {
            var dataTable = new DataTable();
            DataTable result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                using var NpgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                NpgsqlDataAdapter.SelectCommand = cmd;
                NpgsqlDataAdapter.Fill(dataTable);
                sqlTransaction.Commit();
                result = dataTable;
            }
            return result;
        }

        public static DataTable GetDataTable(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            using (var sqlTransaction = connection.BeginTransaction())
            {
                using var NpgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.Transaction = sqlTransaction;
                NpgsqlDataAdapter.SelectCommand = cmd;
                NpgsqlDataAdapter.Fill(dataTable);
                sqlTransaction.Commit();
            }
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string connectionString)
        {
            DataTable dataTable;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                dataTable = GetDataTable(NpgsqlCommand, connectionString);
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string connectionString, Array NpgsqlParameters)
        {
            DataTable dataTable;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataTable = GetDataTable(NpgsqlCommand, connectionString);
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters)
        {
            DataTable dataTable;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                dataTable = GetDataTable(NpgsqlCommand, connectionString);
            }
            return dataTable;
        }

        public static List<T> GetDataTable<T>(NpgsqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(cmd, connectionString));
        }

        public static List<T> GetDataTable<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(cmd, connection, false));
        }

        public static List<T> GetDataTable<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, connectionString));
        }

        public static List<T> GetDataTable<T>(string query, string connectionString, Array NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, connectionString, NpgsqlParameters));
        }

        public static List<T> GetDataTable<T>(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, connectionString, NpgsqlParameters));
        }

        public static T? GetDataRow<T>(NpgsqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(cmd, connectionString));
        }

        public static T? GetDataRow<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(cmd, connection, false));
        }

        public static T? GetDataRow<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, connectionString));
        }

        public static T? GetDataRow<T>(string query, string connectionString, Array NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, connectionString, NpgsqlParameters));
        }

        public static T? GetDataRow<T>(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, connectionString, NpgsqlParameters));
        }

        public static Dictionary<T, U>? GetDictionary<T, U>(string query, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = NpgsqlCommand.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<T, U>? GetDictionary<T, U>(NpgsqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (NpgsqlConnection NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using NpgsqlDataReader NpgsqlDataReader = cmd.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<T, U>? GetDictionary<T, U>(string query, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = NpgsqlCommand.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
                {
                    dictionary.Add((T)((object)NpgsqlDataReader[keyColumnIndex]), (U)((object)NpgsqlDataReader[valueColumnIndex]));
                }
                if (dictionary.Count == 0)
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, U>? GetDictionary<T, U>(NpgsqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (NpgsqlConnection NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<string, string>? GetDictionary(string query, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (NpgsqlConnection NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = NpgsqlCommand.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<string, string>? GetDictionary(NpgsqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (NpgsqlConnection NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2)
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<string, string>? GetDictionary(string query, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Connection = NpgsqlConnection;
                NpgsqlCommand.CommandTimeout = NpgsqlCommandTimeout;
                NpgsqlCommand.CommandText = query;
                using var NpgsqlDataReader = NpgsqlCommand.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<string, string>? GetDictionary(NpgsqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
            using (NpgsqlConnection NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using NpgsqlDataReader NpgsqlDataReader = cmd.ExecuteReader();
                if (NpgsqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && NpgsqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<T, U>? GetDictionaryOfObjects<T, U>(NpgsqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
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
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<T, U>? GetDictionaryOfObjects<T, U>(NpgsqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using NpgsqlDataReader NpgsqlDataReader = cmd.ExecuteReader();
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
                while (NpgsqlDataReader.Read())
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

        public static Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(NpgsqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
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
                while (NpgsqlDataReader.Read())
                {
                    bool flag = false;
                    if (dictionary.TryGetValue((T)((object)NpgsqlDataReader[keyColumnIndex]), out List<U> list2))
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

        public static Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(NpgsqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
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
                while (NpgsqlDataReader.Read())
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

        public static List<List<string>>? GetListListString(NpgsqlCommand cmd, string connectionString)
        {
            var list = new List<List<string>>();
            List<List<string>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
                while (NpgsqlDataReader.Read())
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

        public static List<List<string>>? GetListListString(NpgsqlCommand cmd, string connectionString, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            List<List<string>>? result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                using var NpgsqlDataReader = cmd.ExecuteReader();
                while (NpgsqlDataReader.Read())
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
                            if (array[j])
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

        public static List<List<string>>? GetListListString(string query, string connectionString)
        {
            List<List<string>>? listListString;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                listListString = GetListListString(NpgsqlCommand, connectionString);
            }
            return listListString;
        }

        public static List<List<string>>? GetListListString(string query, string connectionString, string dateFormat)
        {
            List<List<string>>? listListString;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                listListString = GetListListString(NpgsqlCommand, connectionString, dateFormat);
            }
            return listListString;
        }

        public static object GetScalar(NpgsqlCommand cmd, string connectionString)
        {
            object result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                var obj = cmd.ExecuteScalar();
                sqlTransaction.Commit();
                result = obj;
            }
            return result;
        }

        public static object GetScalar(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            object result;
            using (var sqlTransaction = connection.BeginTransaction())
            {
                cmd.Transaction = sqlTransaction;
                result = cmd.ExecuteScalar();
                sqlTransaction.Commit();
            }
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static object GetScalar(string query, string connectionString)
        {
            object scalar;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                scalar = GetScalar(NpgsqlCommand, connectionString);
            }
            return scalar;
        }

        public static object GetScalar(string query, string connectionString, Array NpgsqlParameters)
        {
            object scalar;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                scalar = GetScalar(NpgsqlCommand, connectionString);
            }
            return scalar;
        }

        public static object GetScalar(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters)
        {
            object scalar;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                scalar = GetScalar(NpgsqlCommand, connectionString);
            }
            return scalar;
        }

        public static int ExecuteNonQuery(NpgsqlCommand cmd, string connectionString)
        {
            int result;
            using (var NpgsqlConnection = new NpgsqlConnection(connectionString))
            {
                NpgsqlConnection.Open();
                using var sqlTransaction = NpgsqlConnection.BeginTransaction();
                cmd.Connection = NpgsqlConnection;
                cmd.CommandTimeout = NpgsqlCommandTimeout;
                cmd.Transaction = sqlTransaction;
                int num = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = num;
            }
            return result;
        }

        public static int ExecuteNonQuery(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = NpgsqlCommandTimeout;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                connection.Close();
                connection.Open();
            }
            int result;
            using (var sqlTransaction = connection.BeginTransaction())
            {
                cmd.Transaction = sqlTransaction;
                result = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string connectionString)
        {
            int result;
            using (NpgsqlCommand NpgsqlCommand = new NpgsqlCommand(query))
            {
                result = ExecuteNonQuery(NpgsqlCommand, connectionString);
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string connectionString, Array NpgsqlParameters)
        {
            int result;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                result = ExecuteNonQuery(NpgsqlCommand, connectionString);
            }
            return result;
        }
        public static int ExecuteNonQuery(string query, string connectionString, NpgsqlParameter[] NpgsqlParameters)
        {
            int result;
            using (var NpgsqlCommand = new NpgsqlCommand(query))
            {
                if (NpgsqlParameters != null && NpgsqlParameters.Length != 0)
                {
                    NpgsqlCommand.Parameters.AddRange(NpgsqlParameters);
                }
                result = ExecuteNonQuery(NpgsqlCommand, connectionString);
            }
            return result;
        }


        public static int NpgsqlCommandTimeout { get; set; } = 180;
    }
}
