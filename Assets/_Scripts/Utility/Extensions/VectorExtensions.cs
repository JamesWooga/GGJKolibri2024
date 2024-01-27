using UnityEngine;

namespace Utility.Extensions
{
    public static class VectorExtensions
    {
        public static bool IsCloseToZero(this Vector2 vec, float epsilon = 1E-6f)
        {
            return vec.x.IsCloseToZero(epsilon) && vec.y.IsCloseToZero(epsilon);
        }

        public static float RandomBetweenXAndY(this Vector2 vector2)
        {
            return Random.Range(vector2.x, vector2.y);
        }
    }
}