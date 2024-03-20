using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Utility
{
    public static class PointerUtility
    {
        private static Vector2 GetTouchPosition()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            return Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
			return UnityEngine.Input.touches[0].position;
#endif
        }
        
        public static bool IsPointerOverUIObject()
        {
            var inputPos = GetTouchPosition();
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(inputPos.x, inputPos.y)
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Any(result => result.gameObject.layer == 5);
        }
    }
}