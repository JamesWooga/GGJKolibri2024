using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Extensions
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> GetAllChildren(this Transform transform)
        {
            return transform.Cast<Transform>().ToList();
        }
    }
}