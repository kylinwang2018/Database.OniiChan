using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Database.Aniki.SqlServer
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
        SqlCommand CreateCommand();
    }
}
