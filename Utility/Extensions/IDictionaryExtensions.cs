using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class IDictionaryExtensions
    {
        public static bool HasAny<T, R>(this IDictionary<T, R> collection)
        {
            return (collection != null && collection.Any());
        }

        public static bool HasAny<T, R>(this IDictionary<T, R> collection, Func<KeyValuePair<T, R>, bool> predicate = null)
        {
            return (collection != null && collection.Any(predicate));
        }
    }
}
