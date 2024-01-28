using System.Collections;
using _Scripts.GameState;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Objects
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private ObjectSpawnerIndicator _objectSpawnerIndicator;
        [SerializeField] private DroppedObject[] _possibleSpawns;
        [SerializeField] private bool _isActive = true;
        
        [Header("Tweakable")]
        [SerializeField] private Vector2 _minSpawnPoint;
        [SerializeField] private Vector2 _maxSpawnPoint;
        [SerializeField] private float _cameraZoomHeightIncrease;
        [SerializeField] private Vector2 _initialSpawnDelaySecondsRange;
        [SerializeField] private Vector2 _spawnDurationSecondsRange;
        [SerializeField] private Vector2 _spawnIntervalSecondsRange;
        [SerializeField] private Vector2 _spawnCooldownSecondsRange;

        private Camera _camera;
        private Camera Camera => _camera == null ? _camera = Camera.main : _camera;

        private bool _isStarted;
        private Coroutine _spawning;
        
        private void Start()
        {
            GameEvents.GameEvents.OnCameraZoomUpdated += HandleCameraZoomUpdated;
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
            
            if (!_isActive)
            {
                return;
            }

            if (GameManager.Instance.GameState == GameState.GameState.Play && !_isStarted)
            {
                StartAfterRandomSeconds();
            }
        }

        private void OnDestroy()
        {
            GameEvents.GameEvents.OnCameraZoomUpdated -= HandleCameraZoomUpdated;
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (!_isActive || _isStarted)
            {
                return;
            }

            if (obj == GameState.GameState.Play)
            {
                StartAfterRandomSeconds();    
            }

            if (obj == GameState.GameState.GameOver)
            {
                StopCoroutine(_spawning);
                var objects = FindObjectsOfType<DroppedObject>();
                for (int i = objects.Length - 1; i >= 0; i--)
                {
                    Destroy(objects[i].gameObject);
                }
            }
        }

        private void StartAfterRandomSeconds()
        {
            var randomTime = _initialSpawnDelaySecondsRange.RandomBetweenXAndY();
            _spawning = StartCoroutine(StartSpawning(randomTime));
        }
        
        private IEnumerator StartSpawning(float initialDelay)
        {
            _isStarted = true;
            yield return new WaitForSeconds(initialDelay);
            
            while (GameManager.Instance.GameState == GameState.GameState.Play)
            {
                _objectSpawnerIndicator.Show();
                var spawnDuration = _spawnDurationSecondsRange.RandomBetweenXAndY();
                var time = 0f;
                while (GameManager.Instance.GameState == GameState.GameState.Play)
                {
                    var randomPosition = new Vector2(Random.Range(_minSpawnPoint.x, _maxSpawnPoint.x), Random.Range(_minSpawnPoint.y, _maxSpawnPoint.y));
                    _objectSpawnerIndicator.SetNextSpawn(randomPosition);
                    Spawn(randomPosition);
                    var seconds = _spawnIntervalSecondsRange.RandomBetweenXAndY();
                    yield return new WaitForSeconds(seconds);

                    time += seconds;

                    if (time >= spawnDuration)
                    {
                        break;
                    }
                }

                _objectSpawnerIndicator.Hide();
                yield return new WaitForSeconds(_spawnCooldownSecondsRange.RandomBetweenXAndY());
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void Spawn(Vector2 position)
        {
            var randomObject = _possibleSpawns.RandomElement();
            Instantiate(randomObject, position, Quaternion.Euler(new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360f))));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_minSpawnPoint, 0.5f);
            Gizmos.DrawSphere(_maxSpawnPoint, 0.5f);
            Gizmos.DrawLine(_minSpawnPoint, _maxSpawnPoint);
        }

        private void HandleCameraZoomUpdated()
        {
            _minSpawnPoint.y += _cameraZoomHeightIncrease;
            _maxSpawnPoint.y += _cameraZoomHeightIncrease;
        }
    }
}