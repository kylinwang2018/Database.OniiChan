﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Database.Aniki
{
    public interface ISqlServerDbContext
    {
        /// <summary>
        /// Executes a Transact-SQL statement against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd">A <see cref="SqlCommand"/> object that contain the sql text</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteNonQuery(SqlCommand cmd);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd">A <see cref="SqlCommand"/> object that contain the sql text</param>
        /// <param name="connection">The connection will execute that command</param>
        /// <param name="closeWhenComplete">Close the connection when it is true</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);

        /// <summary>
        /// Executes a Transact-SQL statement against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The sql command in plain text</param>
        /// <param name="commandType">Determine the type of the command, whether is Text or StoredProcedure</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteNonQuery(string query, CommandType commandType);

        /// <summary>
        /// Executes a Transact-SQL statement with parameters against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The sql command in plain text</param>
        /// <param name="commandType">Determine the type of the command, whether is Text or StoredProcedure</param>
        /// <param name="sqlParameters">The parameters</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteNonQuery(string query, CommandType commandType, Array sqlParameters);

        /// <summary>
        /// Executes a Transact-SQL statement with parameters against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The sql command in plain text</param>
        /// <param name="commandType">Determine the type of the command, whether is Text or StoredProcedure</param>
        /// <param name="sqlParameters">The parameters</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteNonQuery(string query, CommandType commandType, SqlParameter[] sqlParameters);

        /// <summary>
        /// Executes a Transact-SQL statement against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd">A <see cref="SqlCommand"/> object that contain the sql text</param>
        /// <returns>The number of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(SqlCommand cmd);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd">A <see cref="SqlCommand"/> object that contain the sql text</param>
        /// <param name="connection">The connection will execute that command</param>
        /// <param name="closeWhenComplete">Close the connection when it is true</param>
        /// <returns>The number of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);

        /// <summary>
        /// Executes a Transact-SQL statement against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The sql command in plain text</param>
        /// <param name="commandType">Determine the type of the command, whether is Text or StoredProcedure</param>
        /// <returns>The number of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(string query, CommandType commandType);

        /// <summary>
        /// Executes a Transact-SQL statement with parameters against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The sql command in plain text</param>
        /// <param name="commandType">Determine the type of the command, whether is Text or StoredProcedure</param>
        /// <param name="sqlParameters">The parameters</param>
        /// <returns>The number of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, Array sqlParameters);

        /// <summary>
        /// Executes a Transact-SQL statement with parameters against a new connection and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The sql command in plain text</param>
        /// <param name="commandType">Determine the type of the command, whether is Text or StoredProcedure</param>
        /// <param name="sqlParameters">The parameters</param>
        /// <returns>The number of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, SqlParameter[] sqlParameters);
        IDataReader ExecuteReader(SqlCommand cmd);
        IDataReader ExecuteReader(SqlCommand cmd, SqlConnection connection);
        IDataReader ExecuteReader(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters);
        IDataReader ExecuteReader(string query, CommandType commandType);
        IDataReader ExecuteReader(string query, CommandType commandType, SqlParameter[] sqlParameters);
        Task<IDataReader> ExecuteReaderAsync(SqlCommand cmd);
        Task<IDataReader> ExecuteReaderAsync(SqlCommand cmd, SqlConnection connection);
        Task<IDataReader> ExecuteReaderAsync(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters);
        Task<IDataReader> ExecuteReaderAsync(string query, CommandType commandType);
        Task<IDataReader> ExecuteReaderAsync(string query, CommandType commandType, SqlParameter[] sqlParameters);
        IDataReader ExecuteReaderSequential(SqlCommand cmd);
        IDataReader ExecuteReaderSequential(SqlCommand cmd, SqlConnection connection);
        IDataReader ExecuteReaderSequential(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters);
        IDataReader ExecuteReaderSequential(string query, CommandType commandType);
        IDataReader ExecuteReaderSequential(string query, CommandType commandType, SqlParameter[] sqlParameters);
        Task<IDataReader> ExecuteReaderSequentialAsync(SqlCommand cmd);
        Task<IDataReader> ExecuteReaderSequentialAsync(SqlCommand cmd, SqlConnection connection);
        Task<IDataReader> ExecuteReaderSequentialAsync(SqlConnection connection, SqlTransaction? transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters);
        Task<IDataReader> ExecuteReaderSequentialAsync(string query, CommandType commandType);
        Task<IDataReader> ExecuteReaderSequentialAsync(string query, CommandType commandType, SqlParameter[] sqlParameters);
        List<string> GetColumnToString(SqlCommand cmd, int columnIndex = 0);
        List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false);
        List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false);
        List<string> GetColumnToString(SqlCommand cmd, string columnName);
        Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, int columnIndex = 0);
        Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false);
        Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false);
        Task<List<string>> GetColumnToStringAsync(SqlCommand cmd, string columnName);
        T? GetDataRow<T>(SqlCommand cmd) where T : class, new();
        T? GetDataRow<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        T? GetDataRow<T>(string query, CommandType commandType) where T : class, new();
        T? GetDataRow<T>(string query, CommandType commandType, Array sqlParameters) where T : class, new();
        T? GetDataRow<T>(string query, CommandType commandType, SqlParameter[] sqlParameters) where T : class, new();
        Task<T?> GetDataRowAsync<T>(SqlCommand cmd) where T : class, new();
        Task<T?> GetDataRowAsync<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        Task<T?> GetDataRowAsync<T>(string query, CommandType commandType) where T : class, new();
        Task<T?> GetDataRowAsync<T>(string query, CommandType commandType, Array sqlParameters) where T : class, new();
        Task<T?> GetDataRowAsync<T>(string query, CommandType commandType, SqlParameter[] sqlParameters) where T : class, new();
        DataTable GetDataTable(SqlCommand cmd);
        DataTable GetDataTable(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        DataTable GetDataTable(string query, CommandType commandType);
        DataTable GetDataTable(string query, CommandType commandType, Array sqlParameters);
        DataTable GetDataTable(string query, CommandType commandType, SqlParameter[] sqlParameters);
        List<T> GetDataTable<T>(SqlCommand cmd) where T : class, new();
        List<T> GetDataTable<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        List<T> GetDataTable<T>(string query, CommandType commandType) where T : class, new();
        List<T> GetDataTable<T>(string query, CommandType commandType, Array sqlParameters) where T : class, new();
        List<T> GetDataTable<T>(string query, CommandType commandType, SqlParameter[] sqlParameters) where T : class, new();
        Task<DataTable> GetDataTableAsync(SqlCommand cmd);
        Task<DataTable> GetDataTableAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        Task<DataTable> GetDataTableAsync(string query, CommandType commandType);
        Task<DataTable> GetDataTableAsync(string query, CommandType commandType, Array sqlParameters);
        Task<DataTable> GetDataTableAsync(string query, CommandType commandType, SqlParameter[] sqlParameters);
        Task<List<T>> GetDataTableAsync<T>(SqlCommand cmd) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, Array sqlParameters) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, SqlParameter[] sqlParameters) where T : class, new();
        Dictionary<string, string>? GetDictionary(SqlCommand cmd);
        Dictionary<string, string>? GetDictionary(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Dictionary<string, string>? GetDictionary(string query, CommandType commandType);
        Dictionary<string, string>? GetDictionary(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd);
        Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType);
        Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<string, string>?> GetDictionaryAsync(SqlCommand cmd);
        Task<Dictionary<string, string>?> GetDictionaryAsync(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType);
        Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(SqlCommand cmd);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new();
        Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new();
        Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new();
        Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new();
        Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new();
        Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new();
        Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new();
        Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new();
        List<List<string>>? GetListListString(SqlCommand cmd);
        List<List<string>>? GetListListString(SqlCommand cmd, string dateFormat);
        List<List<string>>? GetListListString(string query, CommandType commandType);
        List<List<string>>? GetListListString(string query, CommandType commandType, string dateFormat);
        Task<List<List<string>>?> GetListListStringAsync(SqlCommand cmd);
        Task<List<List<string>>?> GetListListStringAsync(SqlCommand cmd, string dateFormat);
        Task<List<List<string>>?> GetListListStringAsync(string query, CommandType commandType);
        Task<List<List<string>>?> GetListListStringAsync(string query, CommandType commandType, string dateFormat);
        List<T>? GetListOf<T>(SqlCommand cmd);
        List<T>? GetListOf<T>(SqlCommand cmd, SqlConnection connection);
        List<T>? GetListOf<T>(string query, CommandType commandType);
        List<T>? GetListOf<T>(string query, CommandType commandType, SqlParameter[] sqlParameters);
        Task<List<T>?> GetListOfAsync<T>(SqlCommand cmd);
        Task<List<T>?> GetListOfAsync<T>(SqlCommand cmd, SqlConnection connection);
        Task<List<T>?> GetListOfAsync<T>(string query, CommandType commandType);
        Task<List<T>?> GetListOfAsync<T>(string query, CommandType commandType, SqlParameter[] sqlParameters);
        object GetScalar(SqlCommand cmd);
        object GetScalar(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        object GetScalar(string query, CommandType commandType);
        object GetScalar(string query, CommandType commandType, Array sqlParameters);
        object GetScalar(string query, CommandType commandType, SqlParameter[] sqlParameters);
        Task<object> GetScalarAsync(SqlCommand cmd);
        Task<object> GetScalarAsync(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        Task<object> GetScalarAsync(string query, CommandType commandType);
        Task<object> GetScalarAsync(string query, CommandType commandType, Array sqlParameters);
        Task<object> GetScalarAsync(string query, CommandType commandType, SqlParameter[] sqlParameters);
    }
}