using System.Collections.Generic;
using System.Linq;

namespace WakEncyclopedie
{
    /// <summary>
    /// Manage pagination with LINQ
    /// <para>Source : https://stackoverflow.com/questions/6185159/linq-and-pagination/6185236#6185236 </para>
    /// </summary>
    public static class PagingExtensions
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}
