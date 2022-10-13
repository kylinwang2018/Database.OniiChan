using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Database.Aniki.Models
{
    public class DbParam
    {
        public string? ParameterName { get; set; }
        public DbType Type { get; set; }
        public object? Value { get; set; }
        public DbParam(string ParameterName, object Value, DbType Type)
        {
            this.ParameterName = ParameterName;
            this.Value = Value;
            this.Type = Type;
        }
        public DbParam()
        {
        }
    }
}
