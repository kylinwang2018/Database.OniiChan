using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Database.Aniki
{
    public interface ISqlServerDbContext
    {
        int ExecuteNonQuery(SqlCommand cmd);
        int ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        int ExecuteNonQuery(string query);
        int ExecuteNonQuery(string query, Array sqlParameters);
        int ExecuteNonQuery(string query, SqlParameter[] sqlParameters);
        List<T> GetColumn<T>(SqlCommand cmd, int columnIndex = 0);
        List<T> GetColumn<T>(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false);
        List<T> GetColumn<T>(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false);
        List<T> GetColumn<T>(SqlCommand cmd, string columnName);
        List<string> GetColumnToString(SqlCommand cmd, int columnIndex = 0);
        List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false);
        List<string> GetColumnToString(SqlCommand cmd, SqlConnection connection, string columnName, bool closeWhenComplete = false);
        List<string> GetColumnToString(SqlCommand cmd, string columnName);
        T? GetDataRow<T>(SqlCommand cmd) where T : class, new();
        T? GetDataRow<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        T? GetDataRow<T>(string query) where T : class, new();
        T? GetDataRow<T>(string query, Array sqlParameters) where T : class, new();
        T? GetDataRow<T>(string query, SqlParameter[] sqlParameters) where T : class, new();
        DataSet GetDataSet(SqlCommand cmd, SqlConnection connection, string[] tableNames, bool closeWhenComplete = false);
        DataSet GetDataSet(SqlCommand cmd, string[] tableNames);
        DataSet GetDataSet(string query, string[] tableNames);
        DataSet GetDataSet(string query, string[] tableNames, Array sqlParameters);
        DataSet GetDataSet(string query, string[] tableNames, SqlParameter[] sqlParameters);
        DataTable GetDataTable(SqlCommand cmd);
        DataTable GetDataTable(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        DataTable GetDataTable(string query);
        DataTable GetDataTable(string query, Array sqlParameters);
        DataTable GetDataTable(string query, SqlParameter[] sqlParameters);
        List<T> GetDataTable<T>(SqlCommand cmd) where T : class, new();
        List<T> GetDataTable<T>(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        List<T> GetDataTable<T>(string query) where T : class, new();
        List<T> GetDataTable<T>(string query, Array sqlParameters) where T : class, new();
        List<T> GetDataTable<T>(string query, SqlParameter[] sqlParameters) where T : class, new();
        Dictionary<string, string>? GetDictionary(SqlCommand cmd);
        Dictionary<string, string>? GetDictionary(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Dictionary<string, string>? GetDictionary(string query);
        Dictionary<string, string>? GetDictionary(string query, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd);
        Dictionary<T, U>? GetDictionary<T, U>(SqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, U>? GetDictionary<T, U>(string query);
        Dictionary<T, U>? GetDictionary<T, U>(string query, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new();
        Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new();
        Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, int keyColumnIndex) where U : class, new();
        Dictionary<T, U>? GetDictionaryOfObjects<T, U>(SqlCommand cmd, string keyColumnName) where U : class, new();
        List<List<string>>? GetListListString(SqlCommand cmd);
        List<List<string>>? GetListListString(SqlCommand cmd, string dateFormat);
        List<List<string>>? GetListListString(string query);
        List<List<string>>? GetListListString(string query, string dateFormat);
        object GetScalar(SqlCommand cmd);
        object GetScalar(SqlCommand cmd, SqlConnection connection, bool closeWhenComplete = false);
        object GetScalar(string query);
        object GetScalar(string query, Array sqlParameters);
        object GetScalar(string query, SqlParameter[] sqlParameters);
    }
}