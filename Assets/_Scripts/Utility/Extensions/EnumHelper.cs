using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extensions
{
    public static class EnumHelper<T>
    {
        private static IEnumerable<T> _enumerable;

        public static IEnumerable<T> Iterator => _enumerable ?? (_enumerable = Enum.GetValues(typeof(T)).Cast<T>());

        public static IEnumerable<T> IteratorExcept(params T[] valuesToExclude)
        {
            return Iterator.Where(x => !valuesToExclude.Contains(x)).ToList();
        }

        public static int Length => Enum.GetNames(typeof(T)).Length;

        public static void ForEach(Action<T> action)
        {
            EnumHelper<T>.Iterator.ForEach(action);
        }
    }
}