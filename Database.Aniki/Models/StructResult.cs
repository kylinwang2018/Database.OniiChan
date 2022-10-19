using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki.Models
{
    public class StructResult<T> where T : struct
    {
        public T? Value { get; set; }
        public long ExecutionTime { get; set; }
    }
}
