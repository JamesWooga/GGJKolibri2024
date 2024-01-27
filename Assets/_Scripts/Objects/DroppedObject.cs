using UnityEngine;

namespace _Scripts.Objects
{
    public class DroppedObject : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Death":
                    Destroy(gameObject);
                    break;
                case "PlayerLeft":
                    Debug.LogError($"Hit player left");
                    break;
                case "PlayerRight":
                    Debug.LogError($"Hit player right");
                    break;
            }
        }
    }
}