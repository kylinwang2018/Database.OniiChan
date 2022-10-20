using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace Database.Aniki.SqlServer
{
    public static class ParameterFactory
    {
        public static SqlParameter Create(string paramName, SqlDbType dbType, object value)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);
            if (value == null) param.Value = DBNull.Value;
            else if (dbType == SqlDbType.UniqueIdentifier)
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

        public static SqlParameter Create(string paramName, SqlDbType dbType, int size, object value)
        {
            SqlParameter param = new SqlParameter(paramName, dbType, size);
            if (value == null) param.Value = DBNull.Value;
            else param.Value = value;
            return param;
        }
    }
}
