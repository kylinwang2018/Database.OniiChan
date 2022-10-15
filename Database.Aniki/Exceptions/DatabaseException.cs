using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Database.Aniki.Exceptions
{
    public class DatabaseException : DbException
    {
        public DatabaseException() : base()
        {
        }

        public DatabaseException(string message) : base(message)
        {
        }

        public DatabaseException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
