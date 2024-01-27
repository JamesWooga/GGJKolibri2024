using _Scripts.GameState;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Cameras
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _initialZoomDuration;
        [SerializeField] private float _zoomBuffer;
        [SerializeField] private float _menuZoom;
        [SerializeField] private float _playZoom;

        private float _maxHeight;
        
        private void Start()
        {
            GameEvents.GameEvents.OnObstacleCaught += HandleObstacleCaught;
            GameStateManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
            _camera.orthographicSize = _menuZoom;
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

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.Play)
            {
                _camera.DOOrthoSize(_playZoom, _initialZoomDuration);
            }
            else if (obj == GameState.GameState.Menu)
            {
                _camera.DOOrthoSize(_menuZoom, _initialZoomDuration);
            }
        }
    }
}