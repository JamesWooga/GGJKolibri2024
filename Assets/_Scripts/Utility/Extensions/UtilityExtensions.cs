using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extensions
{
    public static class UtilityExtensions
    {
        public static List<T> TimesSelect<T>(this int amount, Func<T> mapper)
        {
            var result = new List<T>(amount);
            for (var i = 0; i < amount; i++)
                result.Add(mapper());
            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
        
        public static void ForEachWithIndex<T>(this IEnumerable<T> enumeration, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in enumeration)
            {
                action(e, i++);
            }
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            return enumerable == null || enumerable.GetEnumerator().MoveNext() == false;
        }

        public static int IndexOf<T>(this IEnumerable<T> obj, T value)
        {
            return obj.IndexOf(value, null);
        }

        public static int IndexOf<T>(this IEnumerable<T> obj, T value, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var found = obj
                .Select((a, i) => new { a, i })
                .FirstOrDefault(x => comparer.Equals(x.a, value));
            return found == null ? -1 : found.i;
        }

        public static bool IsEven(this int val)
        {
            return val % 2 == 0;
        }

        public static bool IsOdd(this int val)
        {
            return val % 2 == 1;
        }

        public static void IfPresent<T>(this T? value, Action<T> action)
            where T : struct
        {
            if (value.HasValue)
                action(value.Value);
        }

        public static string ToFormattedString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, 
            Func<TKey, string> keyMapper, Func<TValue, string> valueMapper, string elementSeparator = ", ", string keyValueSeparator = "=")
        {
            return dictionary == null ? "null" : string.Format("{0}{2}{1}", "{", "}", string.Join(elementSeparator,
                dictionary.Select(keyValue => string.Format("{1}{0}{2}", keyValueSeparator, keyMapper(keyValue.Key),
                    valueMapper(keyValue.Value)))
                    .ToArray()));
        }

        public static string ToFormattedString<TValue>(this IDictionary<string, TValue> dictionary,
            Func<TValue, string> valueMapper, string elementSeparator = ", ", string keyValueSeparator = "=")
        {
            return dictionary.ToFormattedString(key => key, valueMapper, elementSeparator, keyValueSeparator);
        }
        
        public static string ToFormattedString(this IDictionary<string, string> dictionary,
            string elementSeparator = ", ", string keyValueSeparator = "=")
        {
            return dictionary.ToFormattedString(value => value, elementSeparator, keyValueSeparator);
        }

        public static string ToFormattedString<TValue>(this IEnumerable<TValue> list,
            Func<TValue, string> mapper, string separator = ", ")
        {
            return list == null ? "null" : string.Format("{0}{2}{1}", "{", "}", string.Join(separator, list.Select(mapper).ToArray()));
        }

        public static string ToFormattedString(this IEnumerable<string> list, string separator = ", ")
        {
            return list == null ? "null" : string.Format("{0}{2}{1}", "{", "}", string.Join(separator, list.ToArray()));
        }
    }
}