using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace Database.Aniki.SqlServer
{
    public static class ParameterFactory
    {
        /// <summary>
        ///     Create a new <see cref="SqlParameter"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="dbType">The type of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Create a new <see cref="SqlParameter"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="dbType">The type of the parameter</param>
        /// <param name="size"></param>
        /// <param name="value">The value of the parameter</param>
        /// <returns></returns>
        public static SqlParameter Create(string paramName, SqlDbType dbType, int size, object value)
        {
            SqlParameter param = new SqlParameter(paramName, dbType, size);
            if (value == null) param.Value = DBNull.Value;
            else param.Value = value;
            return param;
        }
    }
}
