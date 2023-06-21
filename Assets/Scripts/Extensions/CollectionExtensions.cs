using System;
using System.Collections.Generic;

namespace PyramidSolitaire.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddTo<T>(this IEnumerable<T> collection, ICollection<T> other)
        {
            foreach (var item in collection)
                other.Add(item);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }
    }
}