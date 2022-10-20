using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Database.Aniki
{
    public static class DataReaderExtension
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader,
                                       Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        public static void Select(this IDataReader reader,
                                       Func<IDataReader> projection)
        {
            while (reader.Read())
            {
                projection();
            }
        }

    }
}
