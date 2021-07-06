using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static class RequestProviderExtensions
    {
        public static IEnumerable<T[]> CartesianProduct<T>(this IEnumerable<T> vector1, IEnumerable<T> vector2)
        {
            var product = from item1 in vector1
                from item2 in vector2
                select new T[]
                {
                    item1, item2,
                };
            return product;
        }
    }
}
