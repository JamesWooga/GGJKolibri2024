using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Utility.Extensions
{
    public static class EnumerableExtensions
    {
        public static T RandomElement<T>(this T[] arr)
        {
            return arr[Random.Range(0, arr.Length)];
        }
        
        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        
        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}