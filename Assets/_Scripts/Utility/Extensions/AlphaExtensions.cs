using UnityEngine;
using UnityEngine.UI;

namespace Utility.Extensions
{
    public static class AlphaExtensions
    {
        public static void SetAlpha(this Image target, float alpha)
        {
            var color = target.color;
            color.a = alpha;
            target.color = color;
        }

        public static void SetAlpha(this SpriteRenderer target, float alpha)
        {
            var color = target.color;
            color.a = alpha;
            target.color = color;
        }
    }
}