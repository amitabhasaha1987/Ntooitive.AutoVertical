using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class ListExtensions
    {
        public static bool HasAny<T>(this List<T> list)
        {
            return (list != null && list.Any());
        }
    }
}
