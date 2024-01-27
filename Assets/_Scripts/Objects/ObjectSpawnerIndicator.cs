using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Objects
{
    public class ObjectSpawnerIndicator : MonoBehaviour
    {
        [Header("Graphics")]
        [SerializeField] private RectTransform _spawnIndicator;
        [SerializeField] private float _yPosition;
        [SerializeField] private Transform _leftmostTransform;
        [SerializeField] private Transform _rightmostTransform;
        
        private Camera _camera;
        private Camera Camera => _camera == null ? _camera = Camera.main : _camera;
        
        public void SetNextSpawn(Vector2 worldPosition, float minX, float maxX)
        {         
            var leftPoint = Camera.WorldToScreenPoint(_leftmostTransform.position);
            var rightPoint = Camera.WorldToScreenPoint(_rightmostTransform.position);
            var newPos = worldPosition.x.Remap(minX, maxX, leftPoint.x, rightPoint.x);
            _spawnIndicator.position = new Vector3(newPos, _yPosition, 0f);
        }
    }
}