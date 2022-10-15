using Database.Aniki.DataManipulators;
using Database.Aniki.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Database.Aniki.DataManipulators
{
    internal static class SqlServerHelper
    {
        public static List<string> GetColumnToString(SqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connectionStringName), columnIndex);
        }

        public static List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static List<string> GetColumnToString(SqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connectionStringName), columnName);
        }

        public static List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListString(GetDataTable(cmd, connection, closeWhenComplete), columnName);
        }

        public static List<T> GetColumn<T>(SqlCommand cmd, string connectionStringName, int columnIndex = 0)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connectionStringName), columnIndex);
        }

        public static List<T> GetColumn<T>(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connection, closeWhenComplete), columnIndex);
        }

        public static List<T> GetColumn<T>(SqlCommand cmd, string connectionStringName, string columnName)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connectionStringName), columnName);
        }

        public static List<T> GetColumn<T>(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return DataTableHelper.DataTableToListCast<T>(GetDataTable(cmd, connection, closeWhenComplete), columnName);
        }
        public static DataSet GetDataSet(SqlCommand cmd, string connectionString, string[] tableNames)
        {
            var dataSet = new DataSet();
            DataSet result;
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
                result = dataSet;
            }
            return result;
        }

        public static DataSet GetDataSet(SqlCommand cmd, SqlConnection connection, string[] tableNames, bool closeWhenComplete = false)
        {
            var dataSet = new DataSet();
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
                for (int i = 0; i < tableNames.Length; i++)
                {
                    sqlDataAdapter.TableMappings.Add("Table" + ((i > 0) ? i.ToString() : ""), tableNames[i]);
                }
                sqlDataAdapter.Fill(dataSet);
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
            using (var sqlCommand = new SqlCommand(query))
            {
                dataSet = GetDataSet(sqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static DataSet GetDataSet(string query, string connectionString, string[] tableNames, Array sqlParameters)
        {
            DataSet dataSet;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataSet = GetDataSet(sqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static DataSet GetDataSet(string query, string connectionString, string[] tableNames, SqlParameter[] sqlParameters)
        {
            DataSet dataSet;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataSet = GetDataSet(sqlCommand, connectionString, tableNames);
            }
            return dataSet;
        }

        public static DataTable GetDataTable(SqlCommand cmd, string connectionString)
        {
            var dataTable = new DataTable();
            DataTable result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                using var sqlDataAdapter = new SqlDataAdapter();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);
                sqlTransaction.Commit();
                result = dataTable;
            }
            return result;
        }

        public static DataTable GetDataTable(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            var dataTable = new DataTable();
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
            if (closeWhenComplete)
            {
                connection.Close();
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string connectionString)
        {
            DataTable dataTable;
            using (var sqlCommand = new SqlCommand(query))
            {
                dataTable = GetDataTable(sqlCommand, connectionString);
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string connectionString, Array sqlParameters)
        {
            DataTable dataTable;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataTable = GetDataTable(sqlCommand, connectionString);
            }
            return dataTable;
        }

        public static DataTable GetDataTable(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            DataTable dataTable;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                dataTable = GetDataTable(sqlCommand, connectionString);
            }
            return dataTable;
        }

        public static List<T> GetDataTable<T>(SqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(cmd, connectionString));
        }

        public static List<T> GetDataTable<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(cmd, connection, false));
        }

        public static List<T> GetDataTable<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, connectionString));
        }

        public static List<T> GetDataTable<T>(string query, string connectionString, Array sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, connectionString, sqlParameters));
        }

        public static List<T> GetDataTable<T>(string query, string connectionString, SqlParameter[] sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataTableToList<T>(GetDataTable(query, connectionString, sqlParameters));
        }

        public static T? GetDataRow<T>(SqlCommand cmd, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(cmd, connectionString));
        }

        public static T? GetDataRow<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(cmd, connection, false));
        }

        public static T? GetDataRow<T>(string query, string connectionString) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, connectionString));
        }

        public static T? GetDataRow<T>(string query, string connectionString, Array sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, connectionString, sqlParameters));
        }

        public static T? GetDataRow<T>(string query, string connectionString, SqlParameter[] sqlParameters) where T : class, new()
        {
            return DataTableHelper.DataRowToT<T>(GetDataTable(query, connectionString, sqlParameters));
        }

        public static Dictionary<T, U>? GetDictionary<T, U>(string query, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<string, string>? GetDictionary(SqlCommand cmd, string connectionString)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<string, string>? GetDictionary(SqlCommand cmd, string connectionString, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            Dictionary<string, string>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, U>();
            Dictionary<T, U>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string connectionString, int keyColumnIndex) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string connectionString, string keyColumnName) where U : class, new()
        {
            var dictionary = new Dictionary<T, List<U>>();
            Dictionary<T, List<U>>? result;
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
                    result = null;
                else
                    result = dictionary;
            }
            return result;
        }

        public static List<List<string>>? GetListListString(SqlCommand cmd, string connectionString)
        {
            var list = new List<List<string>>();
            List<List<string>>? result;
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
                    result = null;
                else
                    result = list;
            }
            return result;
        }

        public static List<List<string>>? GetListListString(SqlCommand cmd, string connectionString, string dateFormat)
        {
            var list = new List<List<string>>();
            bool flag = true;
            bool[]? array = null;
            List<List<string>>? result;
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
                    result = null;
                else
                    result = list;
            }
            return result;
        }

        public static List<List<string>>? GetListListString(string query, string connectionString)
        {
            List<List<string>>? listListString;
            using (var sqlCommand = new SqlCommand(query))
            {
                listListString = GetListListString(sqlCommand, connectionString);
            }
            return listListString;
        }

        public static List<List<string>>? GetListListString(string query, string connectionString, string dateFormat)
        {
            List<List<string>>? listListString;
            using (var sqlCommand = new SqlCommand(query))
            {
                listListString = GetListListString(sqlCommand, connectionString, dateFormat);
            }
            return listListString;
        }

        public static object GetScalar(SqlCommand cmd, string connectionString)
        {
            object result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                var obj = cmd.ExecuteScalar();
                sqlTransaction.Commit();
                result = obj;
            }
            return result;
        }

        public static object GetScalar(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
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
            using (var sqlCommand = new SqlCommand(query))
            {
                scalar = GetScalar(sqlCommand, connectionString);
            }
            return scalar;
        }

        public static object GetScalar(string query, string connectionString, Array sqlParameters)
        {
            object scalar;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                scalar = GetScalar(sqlCommand, connectionString);
            }
            return scalar;
        }

        public static object GetScalar(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            object scalar;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                scalar = GetScalar(sqlCommand, connectionString);
            }
            return scalar;
        }

        public static int ExecuteNonQuery(SqlCommand cmd, string connectionString)
        {
            int result;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using var sqlTransaction = sqlConnection.BeginTransaction();
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = SQLCommandTimeout;
                cmd.Transaction = sqlTransaction;
                int num = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = num;
            }
            return result;
        }

        public static int ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = SQLCommandTimeout;
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
            using (SqlCommand sqlCommand = new SqlCommand(query))
            {
                result = ExecuteNonQuery(sqlCommand, connectionString);
            }
            return result;
        }

        public static int ExecuteNonQuery(string query, string connectionString, Array sqlParameters)
        {
            int result;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                result = ExecuteNonQuery(sqlCommand, connectionString);
            }
            return result;
        }
        public static int ExecuteNonQuery(string query, string connectionString, SqlParameter[] sqlParameters)
        {
            int result;
            using (var sqlCommand = new SqlCommand(query))
            {
                if (sqlParameters != null && sqlParameters.Length != 0)
                {
                    sqlCommand.Parameters.AddRange(sqlParameters);
                }
                result = ExecuteNonQuery(sqlCommand, connectionString);
            }
            return result;
        }


        public static int SQLCommandTimeout { get; set; } = 180;
    }
}
