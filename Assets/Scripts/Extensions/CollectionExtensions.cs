using System.Collections.Generic;

namespace PyramidSolitaire.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddTo<T>(this IEnumerable<T> coll, ICollection<T> other)
        {
            foreach (var item in coll)
                other.Add(item);
        }
    }
}