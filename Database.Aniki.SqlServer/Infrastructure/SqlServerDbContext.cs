using Database.Aniki.DataManipulators;
using Database.Aniki.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Database.Aniki
{
    public class SqlServerDbContext: ISqlServerDbContext
    {
        private readonly IDbContextOptions _options;
        private readonly ILogger<SqlServerDbContext> _logger;

        public SqlServerDbContext(
            IOptions<DbContextOptions> options, ILogger<SqlServerDbContext> logger)
        {
            _options = options.Value;
            SqlServerAsyncHelper.SQLCommandTimeout = options.Value.DbCommandTimeout;
            SqlServerHelper.SQLCommandTimeout = options.Value.DbCommandTimeout;
            _logger = logger;
        }

        #region Sync methods

        public List<string> GetColumnToString(SqlCommand cmd, int columnIndex = 0)
        {
            _logger.LogInformation("Command:\n\t{Command}", cmd.CommandText);
            return SqlServerHelper.GetColumnToString(cmd, _options.ConnectionSting, columnIndex);
        }

        public List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return SqlServerHelper.GetColumnToString(cmd, connection, columnIndex, closeWhenComplete);
        }

        public List<string> GetColumnToString(SqlCommand cmd, string columnName)
        {
            return SqlServerHelper.GetColumnToString(cmd, _options.ConnectionSting, columnName);
        }

        public List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return SqlServerHelper.GetColumnToString(cmd, connection, columnName, closeWhenComplete);
        }

        public List<T> GetColumn<T>(SqlCommand cmd, int columnIndex = 0)
        {
            return SqlServerHelper.GetColumn<T>(cmd, _options.ConnectionSting, columnIndex);
        }
        public List<T> GetColumn<T>(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false)
        {
            return SqlServerHelper.GetColumn<T>(cmd, connection, columnIndex, closeWhenComplete);
        }

        public List<T> GetColumn<T>(SqlCommand cmd, string columnName)
        {
            return SqlServerHelper.GetColumn<T>(cmd, _options.ConnectionSting, columnName);
        }

        public List<T> GetColumn<T>(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false)
        {
            return SqlServerHelper.GetColumn<T>(cmd, connection, columnName, closeWhenComplete);
        }

        public DataSet? GetDataSet(SqlCommand cmd, string[] tableNames)
        {
            var result = SqlServerHelper.GetDataSet(cmd, _options.ConnectionSting, tableNames);
            _logger.LogInformation(
                "\nCommand:\n\t{Command},\nExection Time:{ExecutionTime}", 
                cmd.CommandText,
                result.ExecutionTime.ToString());
            return result.Value;
        }

        public DataSet? GetDataSet(SqlCommand cmd, SqlConnection connection, string[] tableNames, bool closeWhenComplete = false)
        {
            var result = SqlServerHelper.GetDataSet(cmd, connection, tableNames, closeWhenComplete);
            _logger.LogInformation(
                "\nCommand:\n\t{Command},\nExection Time:{ExecutionTime}",
                cmd.CommandText,
                result.ExecutionTime.ToString());
            return result.Value;
        }

        public DataSet? GetDataSet(string query, string[] tableNames)
        {
            var result = SqlServerHelper.GetDataSet(query, _options.ConnectionSting, tableNames);
            _logger.LogInformation(
                "\nCommand:\n\t{Command},\nExection Time:{ExecutionTime}",
                query,
                result.ExecutionTime.ToString());
            return result.Value;
        }

        public DataSet? GetDataSet(string query, string[] tableNames, Array sqlParameters)
        {
            var result = SqlServerHelper.GetDataSet(query, _options.ConnectionSting, tableNames, sqlParameters);
            _logger.LogInformation(
                "\nCommand:\n\t{Command},\nExection Time:{ExecutionTime}",
                query,
                result.ExecutionTime.ToString());
            return result.Value;
        }

        public DataSet GetDataSet(string query, string[] tableNames, SqlParameter[] sqlParameters)
        {
            return SqlServerHelper.GetDataSet(query, _options.ConnectionSting, tableNames, sqlParameters);
        }

        public DataTable GetDataTable(SqlCommand cmd)
        {
            return SqlServerHelper.GetDataTable(cmd, _options.ConnectionSting);
        }

        public DataTable GetDataTable(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            return SqlServerHelper.GetDataTable(cmd, connection, closeWhenComplete);
        }

        public DataTable GetDataTable(string query)
        {
            return SqlServerHelper.GetDataTable(query, _options.ConnectionSting);
        }

        public DataTable GetDataTable(string query, Array sqlParameters)
        {
            return SqlServerHelper.GetDataTable(query, _options.ConnectionSting, sqlParameters);
        }

        public DataTable GetDataTable(string query, SqlParameter[] sqlParameters)
        {
            return SqlServerHelper.GetDataTable(query, _options.ConnectionSting, sqlParameters);
        }

        public List<T> GetDataTable<T>(SqlCommand cmd) where T : class, new()
        {
            return SqlServerHelper.GetDataTable<T>(cmd, _options.ConnectionSting);
        }

        public List<T> GetDataTable<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return SqlServerHelper.GetDataTable<T>(cmd, connection, closeWhenComplete);
        }

        public List<T> GetDataTable<T>(string query) where T : class, new()
        {
            return SqlServerHelper.GetDataTable<T>(query, _options.ConnectionSting);
        }

        public List<T> GetDataTable<T>(string query, Array sqlParameters) where T : class, new()
        {
            return SqlServerHelper.GetDataTable<T>(query, _options.ConnectionSting, sqlParameters);
        }

        public List<T> GetDataTable<T>(string query, SqlParameter[] sqlParameters) where T : class, new()
        {
            return SqlServerHelper.GetDataTable<T>(query, _options.ConnectionSting, sqlParameters);
        }

        public T? GetDataRow<T>(SqlCommand cmd) where T : class, new()
        {
            return SqlServerHelper.GetDataRow<T>(cmd, _options.ConnectionSting);
        }

        public T? GetDataRow<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new()
        {
            return SqlServerHelper.GetDataRow<T>(cmd, connection, closeWhenComplete);
        }

        public T? GetDataRow<T>(string query) where T : class, new()
        {
            return SqlServerHelper.GetDataRow<T>(query, _options.ConnectionSting);
        }

        public T? GetDataRow<T>(string query, Array sqlParameters) where T : class, new()
        {
            return SqlServerHelper.GetDataRow<T>(query, _options.ConnectionSting, sqlParameters);
        }

        public T? GetDataRow<T>(string query, SqlParameter[] sqlParameters) where T : class, new()
        {
            return SqlServerHelper.GetDataRow<T>(query, _options.ConnectionSting, sqlParameters);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(string query)
        {
            return SqlServerHelper.GetDictionary<T, U>(query, _options.ConnectionSting);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd)
        {
            return SqlServerHelper.GetDictionary<T, U>(cmd, _options.ConnectionSting);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(string query, int keyColumnIndex, int valueColumnIndex)
        {
            return SqlServerHelper.GetDictionary<T, U>(query, _options.ConnectionSting, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            return SqlServerHelper.GetDictionary<T, U>(cmd, _options.ConnectionSting, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<string, string>? GetDictionary(string query)
        {
            return SqlServerHelper.GetDictionary(query, _options.ConnectionSting);
        }

        public Dictionary<string, string>? GetDictionary(SqlCommand cmd)
        {
            return SqlServerHelper.GetDictionary(cmd, _options.ConnectionSting);
        }

        public Dictionary<string, string>? GetDictionary(string query, int keyColumnIndex, int valueColumnIndex)
        {
            return SqlServerHelper.GetDictionary(query, _options.ConnectionSting, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<string, string>? GetDictionary(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex)
        {
            return SqlServerHelper.GetDictionary(cmd, _options.ConnectionSting, keyColumnIndex, valueColumnIndex);
        }

        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            return SqlServerHelper.GetDictionaryOfObjects<T, U>(cmd, _options.ConnectionSting, keyColumnIndex);
        }

        public Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new()
        {
            return SqlServerHelper.GetDictionaryOfObjects<T, U>(cmd, _options.ConnectionSting, keyColumnName);
        }

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new()
        {
            return SqlServerHelper.GetDictionaryOfListObjects<T, U>(cmd, _options.ConnectionSting, keyColumnIndex);
        }

        public Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new()
        {
            return SqlServerHelper.GetDictionaryOfListObjects<T, U>(cmd, _options.ConnectionSting, keyColumnName);
        }

        public List<List<string>>? GetListListString(SqlCommand cmd)
        {
            return SqlServerHelper.GetListListString(cmd, _options.ConnectionSting);
        }

        public List<List<string>>? GetListListString(SqlCommand cmd, string dateFormat)
        {
            return SqlServerHelper.GetListListString(cmd, _options.ConnectionSting, dateFormat);
        }

        public List<List<string>>? GetListListString(string query)
        {
            return SqlServerHelper.GetListListString(query, _options.ConnectionSting);
        }

        public List<List<string>>? GetListListString(string query, string dateFormat)
        {
            return SqlServerHelper.GetListListString(query, _options.ConnectionSting, dateFormat);
        }

        public object GetScalar(SqlCommand cmd)
        {
            return SqlServerHelper.GetScalar(cmd, _options.ConnectionSting);
        }

        public object GetScalar(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            return SqlServerHelper.GetScalar(cmd, connection, closeWhenComplete);
        }

        public object GetScalar(string query)
        {
            return SqlServerHelper.GetScalar(query, _options.ConnectionSting);
        }

        public object GetScalar(string query, Array sqlParameters)
        {
            return SqlServerHelper.GetScalar(query, _options.ConnectionSting, sqlParameters);
        }

        public object GetScalar(string query, SqlParameter[] sqlParameters)
        {
            return SqlServerHelper.GetScalar(query, _options.ConnectionSting, sqlParameters);
        }

        public int ExecuteNonQuery(SqlCommand cmd)
        {
            return SqlServerHelper.ExecuteNonQuery(cmd, _options.ConnectionSting);
        }

        public int ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false)
        {
            return SqlServerHelper.ExecuteNonQuery(cmd, connection, closeWhenComplete);
        }

        public int ExecuteNonQuery(string query)
        {
            return SqlServerHelper.ExecuteNonQuery(query, _options.ConnectionSting);
        }

        public int ExecuteNonQuery(string query, Array sqlParameters)
        {
            return SqlServerHelper.ExecuteNonQuery(query, _options.ConnectionSting, sqlParameters);
        }

        public int ExecuteNonQuery(string query, SqlParameter[] sqlParameters)
        {
            return SqlServerHelper.ExecuteNonQuery(query, _options.ConnectionSting, sqlParameters);
        }

        #endregion

        #region Async methods

        #endregion
    }
}
