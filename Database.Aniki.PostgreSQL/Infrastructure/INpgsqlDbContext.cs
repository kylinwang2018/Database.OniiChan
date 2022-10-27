﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Database.Aniki
{
    public interface INpgsqlDbContext
    {
        int ExecuteNonQuery(NpgsqlCommand cmd);
        int ExecuteNonQuery(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false);
        int ExecuteNonQuery(string query, CommandType commandType);
        int ExecuteNonQuery(string query, CommandType commandType, Array NpgsqlParameters);
        int ExecuteNonQuery(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        Task<int> ExecuteNonQueryAsync(NpgsqlCommand cmd);
        Task<int> ExecuteNonQueryAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false);
        Task<int> ExecuteNonQueryAsync(string query, CommandType commandType);
        Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, Array NpgsqlParameters);
        Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        NpgsqlDataReader ExecuteReader(NpgsqlCommand cmd);
        NpgsqlDataReader ExecuteReader(NpgsqlCommand cmd, NpgsqlConnection connection);
        NpgsqlDataReader ExecuteReader(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters);
        NpgsqlDataReader ExecuteReader(string query, CommandType commandType);
        NpgsqlDataReader ExecuteReader(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlCommand cmd);
        Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlCommand cmd, NpgsqlConnection connection);
        Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters);
        Task<NpgsqlDataReader> ExecuteReaderAsync(string query, CommandType commandType);
        Task<NpgsqlDataReader> ExecuteReaderAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        NpgsqlDataReader ExecuteReaderSequential(NpgsqlCommand cmd);
        NpgsqlDataReader ExecuteReaderSequential(NpgsqlCommand cmd, NpgsqlConnection connection);
        NpgsqlDataReader ExecuteReaderSequential(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters);
        NpgsqlDataReader ExecuteReaderSequential(string query, CommandType commandType);
        NpgsqlDataReader ExecuteReaderSequential(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(NpgsqlCommand cmd);
        Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(NpgsqlCommand cmd, NpgsqlConnection connection);
        Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(NpgsqlConnection connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters);
        Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(string query, CommandType commandType);
        Task<NpgsqlDataReader> ExecuteReaderSequentialAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        List<string> GetColumnToString(NpgsqlCommand cmd, int columnIndex = 0);
        List<string> GetColumnToString(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false);
        List<string> GetColumnToString(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false);
        List<string> GetColumnToString(NpgsqlCommand cmd, string columnName);
        Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, int columnIndex = 0);
        Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, NpgsqlConnection connection, int columnIndex = 0, bool closeWhenComplete = false);
        Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, NpgsqlConnection connection, string columnName, bool closeWhenComplete = false);
        Task<List<string>> GetColumnToStringAsync(NpgsqlCommand cmd, string columnName);
        T? GetDataRow<T>(NpgsqlCommand cmd) where T : class, new();
        T? GetDataRow<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        T? GetDataRow<T>(string query, CommandType commandType) where T : class, new();
        T? GetDataRow<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new();
        T? GetDataRow<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters) where T : class, new();
        Task<T?> GetDataRowAsync<T>(NpgsqlCommand cmd) where T : class, new();
        Task<T?> GetDataRowAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        Task<T?> GetDataRowAsync<T>(string query, CommandType commandType) where T : class, new();
        Task<T?> GetDataRowAsync<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new();
        Task<T?> GetDataRowAsync<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters) where T : class, new();
        DataTable GetDataTable(NpgsqlCommand cmd);
        DataTable GetDataTable(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false);
        DataTable GetDataTable(string query, CommandType commandType);
        DataTable GetDataTable(string query, CommandType commandType, Array NpgsqlParameters);
        DataTable GetDataTable(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        List<T> GetDataTable<T>(NpgsqlCommand cmd) where T : class, new();
        List<T> GetDataTable<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        List<T> GetDataTable<T>(string query, CommandType commandType) where T : class, new();
        List<T> GetDataTable<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new();
        List<T> GetDataTable<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters) where T : class, new();
        Task<DataTable> GetDataTableAsync(NpgsqlCommand cmd);
        Task<DataTable> GetDataTableAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false);
        Task<DataTable> GetDataTableAsync(string query, CommandType commandType);
        Task<DataTable> GetDataTableAsync(string query, CommandType commandType, Array NpgsqlParameters);
        Task<DataTable> GetDataTableAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        Task<List<T>> GetDataTableAsync<T>(NpgsqlCommand cmd) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, Array NpgsqlParameters) where T : class, new();
        Task<List<T>> GetDataTableAsync<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters) where T : class, new();
        Dictionary<string, string>? GetDictionary(NpgsqlCommand cmd);
        Dictionary<string, string>? GetDictionary(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Dictionary<string, string>? GetDictionary(string query, CommandType commandType);
        Dictionary<string, string>? GetDictionary(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, U>? GetDictionary<T, U>(NpgsqlCommand cmd);
        Dictionary<T, U>? GetDictionary<T, U>(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType);
        Dictionary<T, U>? GetDictionary<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<string, string>?> GetDictionaryAsync(NpgsqlCommand cmd);
        Task<Dictionary<string, string>?> GetDictionaryAsync(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType);
        Task<Dictionary<string, string>?> GetDictionaryAsync(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(NpgsqlCommand cmd);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(NpgsqlCommand cmd, int keyColumnIndex, int valueColumnIndex);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType);
        Task<Dictionary<T, U>?> GetDictionaryAsync<T, U>(string query, CommandType commandType, int keyColumnIndex, int valueColumnIndex);
        Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new();
        Dictionary<T, List<U>>? GetDictionaryOfListObjects<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new();
        Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new();
        Task<Dictionary<T, List<U>>?> GetDictionaryOfListObjectsAsync<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new();
        Dictionary<T, U>? GetDictionaryOfObjects<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new();
        Dictionary<T, U>? GetDictionaryOfObjects<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new();
        Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(NpgsqlCommand cmd, int keyColumnIndex) where U : class, new();
        Task<Dictionary<T, U>?> GetDictionaryOfObjectsAsync<T, U>(NpgsqlCommand cmd, string keyColumnName) where U : class, new();
        List<List<string>>? GetListListString(NpgsqlCommand cmd);
        List<List<string>>? GetListListString(NpgsqlCommand cmd, string dateFormat);
        List<List<string>>? GetListListString(string query, CommandType commandType);
        List<List<string>>? GetListListString(string query, CommandType commandType, string dateFormat);
        Task<List<List<string>>?> GetListListStringAsync(NpgsqlCommand cmd);
        Task<List<List<string>>?> GetListListStringAsync(NpgsqlCommand cmd, string dateFormat);
        Task<List<List<string>>?> GetListListStringAsync(string query, CommandType commandType);
        Task<List<List<string>>?> GetListListStringAsync(string query, CommandType commandType, string dateFormat);
        List<T> GetListOf<T>(NpgsqlCommand cmd);
        List<T> GetListOf<T>(NpgsqlCommand cmd, NpgsqlConnection connection);
        List<T> GetListOf<T>(string query, CommandType commandType);
        List<T> GetListOf<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        Task<List<T>> GetListOfAsync<T>(NpgsqlCommand cmd);
        Task<List<T>> GetListOfAsync<T>(NpgsqlCommand cmd, NpgsqlConnection connection);
        Task<List<T>> GetListOfAsync<T>(string query, CommandType commandType);
        Task<List<T>> GetListOfAsync<T>(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        object? GetScalar(NpgsqlCommand cmd);
        object GetScalar(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false);
        object? GetScalar(string query, CommandType commandType);
        object? GetScalar(string query, CommandType commandType, Array NpgsqlParameters);
        object? GetScalar(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
        Task<object> GetScalarAsync(NpgsqlCommand cmd);
        Task<object> GetScalarAsync(NpgsqlCommand cmd, NpgsqlConnection connection, bool closeWhenComplete = false);
        Task<object> GetScalarAsync(string query, CommandType commandType);
        Task<object> GetScalarAsync(string query, CommandType commandType, Array NpgsqlParameters);
        Task<object> GetScalarAsync(string query, CommandType commandType, params NpgsqlParameter[] NpgsqlParameters);
    }
}