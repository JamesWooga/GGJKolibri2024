using UnityEngine;

namespace ExampleFeatureName
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * (_speed * Time.deltaTime));
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * (_speed * Time.deltaTime));
            }
        }
    }
}