using System;
using System.Collections.Generic;
using System.Linq;

namespace IndexRenderedContent
{
    public static class Extensions
    {
        /// <summary>
        /// Filters items out of a collection that have distinct key values. Key values are determined by <paramref name="keySelector"/>.
        /// </summary>
        /// <typeparam name="TSource">Type of the enumerable object</typeparam>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <param name="source">The items to select from</param>
        /// <param name="keySelector">A function to select key values</param>
        /// <returns>The items that have distinct key values</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}