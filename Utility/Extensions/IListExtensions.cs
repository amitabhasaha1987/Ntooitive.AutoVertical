using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class IListExtensions
    {
        public static bool HasAny<T>(this IList<T> list)
        {
            return (list != null && list.Any());
        }
    }
}
