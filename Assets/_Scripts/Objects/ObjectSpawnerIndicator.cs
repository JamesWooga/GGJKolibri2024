using _Scripts.Cameras;
using UnityEngine;

namespace _Scripts.Objects
{
    public class ObjectSpawnerIndicator : MonoBehaviour
    {
        [Header("Graphics")]
        [SerializeField] private Transform _spawnIndicator;
        [SerializeField] private float _cameraIncreaseAmount;
        [SerializeField] private float _yPosition;
        [SerializeField] private CameraZoom _cameraZoom;
        
        public void SetNextSpawn(Vector2 worldPosition)
        {
            var yPos = (_cameraZoom.Camera.orthographicSize - _cameraZoom.PlayZoom) * _cameraIncreaseAmount; 
            _spawnIndicator.position = new Vector3(worldPosition.x, _yPosition + yPos);
        }

        public void Show()
        {
            _spawnIndicator.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _spawnIndicator.gameObject.SetActive(false);
        }
    }
}