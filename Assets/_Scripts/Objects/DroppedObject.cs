using System.Linq;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.Objects
{
    public class DroppedObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GameObject _childCollider;
        
        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        public void SetTag(string newTag)
        {
            gameObject.tag = newTag;
            _childCollider.tag = newTag;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "DroppedObjectLeft":
                    FindAttachPoint(true);
                    break;
                case "DroppedObjectRight":
                    FindAttachPoint(false);
                    break;
                case "Death":
                    Destroy(gameObject);
                    break;
                case "PlayerLeft":
                case "PlayerRight":
                    TryToAttachToCatchPoint(other);
                    break;
            }
        }

        private void TryToAttachToCatchPoint(Component other)
        {
            if (other.TryGetComponent<PlayerCatchPoint>(out var component))
            {
                component.Attach(this);
            }
        }

        private void FindAttachPoint(bool left)
        {
            var attachPoints = FindObjectsOfType<PlayerCatchPoint>();
            var correct = attachPoints.FirstOrDefault(e => e.Left == left);
            
            if (correct == null)
            {
                Debug.LogError($"Couldn't find attach point. You may need to check there are 2 catch points in the scene");
                return;
            }

            correct.Attach(this);
        }
    }
}