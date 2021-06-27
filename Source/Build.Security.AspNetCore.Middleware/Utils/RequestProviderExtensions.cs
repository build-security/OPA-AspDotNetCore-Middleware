using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class RequestProviderExtensions
    {
                public static IEnumerable<T[]> CartesianProduct<T>(this IEnumerable<T> vector1, IEnumerable<T> vector2)
        {
            var cartesianRes = from v1 in vector1
                from v2 in vector2
                select new T[] { v1, v2 };
            return cartesianRes;
        }
    }
}
