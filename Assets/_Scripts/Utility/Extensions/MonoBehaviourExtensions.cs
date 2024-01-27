using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static IEnumerable<Transform> GetAllChildren(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.transform.GetAllChildren();
        }
        
        public static IEnumerable<T> GetAllChildrenWithComponent<T>(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.transform.GetAllChildren().Select(e => e.GetComponent<T>()).Where(e => e != null);
        }
    }
}