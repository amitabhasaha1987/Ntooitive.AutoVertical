using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool HasAny<T>(this IEnumerable<T> list)
        {
            return (list != null && list.Any());
        }

        public static bool HasAny<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return (list != null && list.Any(predicate));
        }

        public static IEnumerable<T> DistinctBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector)
        {
            var seenKeys = new HashSet<K>();

            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }


        public static int HasCount<T>(this IEnumerable<T> list)
        {
            return list== null ? 0 : ( list.Count());
        }
    }
}
