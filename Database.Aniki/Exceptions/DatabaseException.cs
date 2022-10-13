using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki.Exceptions
{
    public class DatabaseException : Exception
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
