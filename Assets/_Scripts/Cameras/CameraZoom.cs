using DG.Tweening;
using UnityEngine;

namespace _Scripts.Cameras
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _zoomBuffer;

        private float _maxHeight;
        
        private void Awake()
        {
            GameEvents.GameEvents.OnObstacleCaught += HandleObstacleCaught;
        }

        private void OnDestroy()
        {
            GameEvents.GameEvents.OnObstacleCaught -= HandleObstacleCaught;
        }

        private void HandleObstacleCaught(float height)
        {
            if (height > _maxHeight)
            {
                _maxHeight = height;
            }

            if (_maxHeight > _camera.orthographicSize - _zoomBuffer)
            {
                _camera.DOOrthoSize(_camera.orthographicSize + 1, _zoomDuration);
                GameEvents.GameEvents.CameraZoomUpdated();
            }
        }
    }
}