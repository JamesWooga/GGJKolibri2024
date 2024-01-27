using System.Collections;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Objects
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 _minSpawnPoint;
        [SerializeField] private Vector2 _maxSpawnPoint;
        [SerializeField] private float _spawnRateSeconds;
        [SerializeField] private float _afterSpawnDelay;
        [SerializeField] private DroppedObject[] _possibleSpawns;
        [SerializeField] private ObjectSpawnerIndicator _objectSpawnerIndicator;
        [SerializeField] private float _cameraZoomHeightIncrease;

        private void Start()
        {
            StartCoroutine(StartSpawning());
        }

        private void Awake()
        {
            GameEvents.GameEvents.OnCameraZoomUpdated += HandleCameraZoomUpdated;
        }

        private void OnDestroy()
        {
            GameEvents.GameEvents.OnCameraZoomUpdated -= HandleCameraZoomUpdated;
        }

        private IEnumerator StartSpawning()
        {
            while (true)
            {
                var randomPosition = new Vector2(Random.Range(_minSpawnPoint.x, _maxSpawnPoint.x), Random.Range(_minSpawnPoint.y, _maxSpawnPoint.y));
                _objectSpawnerIndicator.SetNextSpawn(randomPosition, _minSpawnPoint.x, _maxSpawnPoint.x);
                yield return new WaitForSeconds(_spawnRateSeconds);
                Spawn(randomPosition);
                yield return new WaitForSeconds(_afterSpawnDelay);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void Spawn(Vector2 position)
        {
            var randomObject = _possibleSpawns.RandomElement();
            Instantiate(randomObject, position, Quaternion.identity);
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