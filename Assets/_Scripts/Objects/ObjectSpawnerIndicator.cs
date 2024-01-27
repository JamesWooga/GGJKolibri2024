using UnityEngine;

namespace _Scripts.Objects
{
    public class ObjectSpawnerIndicator : MonoBehaviour
    {
        [Header("Graphics")]
        [SerializeField] private Transform _spawnIndicatorNew;
        [SerializeField] private float _yPositionNew;
        
        public void SetNextSpawn(Vector2 worldPosition)
        {         
            _spawnIndicatorNew.position = new Vector3(worldPosition.x, _yPositionNew);
        }
    }
}