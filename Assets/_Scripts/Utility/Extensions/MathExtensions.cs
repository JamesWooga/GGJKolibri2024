using System;
using UnityEngine;

namespace Utility.Extensions
{
    public static class MathExtensions
    {
        public static float Remap(this float value, Vector2 fromRange, Vector2 toRange)
        {
            return Remap(value, fromRange.x, fromRange.y, toRange.x, toRange.y);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        public static float Clamp(this float value, float min, float max)
        {
            value = Math.Min(max, value);
            value = Math.Max(min, value);

            return value;
        }
        
        public static double Clamp(this double value, double min, double max)
        {
            value = Math.Min(max, value);
            value = Math.Max(min, value);

            return value;
        }

        public static bool IsCloseToZero(this float value, float epsilon = 1E-6f)
        {
            return Mathf.Abs(value) < epsilon;
        }
        
        public static bool IsCloseToEqual(this double value, double other, double epsilon = 1E-6d)
        {
            return Math.Abs(value - other) < epsilon;
        }

        public static string Minify(this double value)
        {
            return value.ToString("F0");
        }
    }
}