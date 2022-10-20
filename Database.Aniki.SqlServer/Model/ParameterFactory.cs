using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data;
using System.Text;

namespace Database.Aniki.SqlServer
{
    public static class ParameterFactory
    {
        public static SqlParameter CreateParameter(string paramName, SqlDbType dbType, object value)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);
            if (value == null) param.Value = DBNull.Value;
            else if (dbType == SqlDbType.UniqueIdentifier)
            {
                string s = value.ToString();
                if (!String.IsNullOrEmpty(s))
                    param.Value = new SqlGuid(s);
                else
                    param.Value = DBNull.Value;
            }
            else param.Value = value;
            return param;
        }

        public static SqlParameter CreateParameter(string paramName, SqlDbType dbType, int size, object value)
        {
            SqlParameter param = new SqlParameter(paramName, dbType, size);
            if (value == null) param.Value = DBNull.Value;
            else param.Value = value;
            return param;
        }
    }
}
