using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Objects
{
    public class ObjectSpawnerIndicator : MonoBehaviour
    {
        [Header("Graphics")]
        [SerializeField] private RectTransform _spawnIndicator;
        [SerializeField] private float _yPosition;
        
        private Camera _camera;
        private Camera Camera => _camera == null ? _camera = Camera.main : _camera;
        
        public void SetNextSpawn(Vector2 worldPosition, float minX, float maxX)
        {
            var newPos = worldPosition.x.Remap(minX, maxX, -Screen.width, Screen.width);
            _spawnIndicator.anchoredPosition = new Vector2(newPos, _yPosition);
        }
    }
}