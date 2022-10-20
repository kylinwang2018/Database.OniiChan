using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace Database.Aniki.PostgreSQL
{
    public static class ParameterFactory
    {
        public static NpgsqlParameter Create(string paramName, NpgsqlDbType dbType, object value)
        {
            NpgsqlParameter param = new NpgsqlParameter(paramName, dbType);
            if (value == null) param.Value = DBNull.Value;
            else if (dbType == NpgsqlDbType.Uuid)
            {
                string s = value.ToString();
                if (!string.IsNullOrEmpty(s))
                    param.Value = new SqlGuid(s);
                else
                    param.Value = DBNull.Value;
            }
            else param.Value = value;
            return param;
        }

        public static NpgsqlParameter Create(string paramName, NpgsqlDbType dbType, int size, object value)
        {
            NpgsqlParameter param = new NpgsqlParameter(paramName, dbType, size);
            if (value == null) param.Value = DBNull.Value;
            else param.Value = value;
            return param;
        }
    }
}
