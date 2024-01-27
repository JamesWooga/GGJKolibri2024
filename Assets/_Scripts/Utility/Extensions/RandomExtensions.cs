using System;

namespace Utility.Extensions
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float minInclusive, float maxInclusive)
        {
            double val = random.NextDouble() * (maxInclusive - minInclusive) + minInclusive;
            return (float)val;
        }
    }
}