using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki.Models
{
    public class Result<T> where T : class, new()
    {
        public T? Value { get; set; }
        public long ExecutionTime { get; set; }
    }
}
