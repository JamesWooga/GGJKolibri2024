using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace ExampleFeatureName
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _forceAmount;
        [SerializeField] private ForceMode2D _forceMode;
        [SerializeField] private Rigidbody2D _playerBody;
        [SerializeField] private float _angleMultiplier;
        [SerializeField] private Image _moveRightIndicator;
        [SerializeField] private Image _moveLeftIndicator;
        [SerializeField] private Color _pressedColor;
        [SerializeField] private Color _notPressedColor;
        
        private void Update()
        {
            var isPressingLeft = Input.GetKey(KeyCode.A);
            var isPressingRight = Input.GetKey(KeyCode.D);
            
            if (isPressingLeft)
            {
                _rigidbody.AddForce(Vector2.left * _forceAmount, _forceMode);
            }
            
            if (isPressingRight)
            {
                _rigidbody.AddForce(Vector2.right * _forceAmount, _forceMode);
            }

            _moveLeftIndicator.color = isPressingLeft ? _pressedColor : _notPressedColor;
            _moveRightIndicator.color = isPressingRight ? _pressedColor : _notPressedColor;
        }

        private void FixedUpdate()
        {
            var signedAngle = Vector2.SignedAngle(Vector2.down, _playerBody.position);
            _rigidbody.AddForce(Vector2.right * (signedAngle * _angleMultiplier), _forceMode);
        }
    }
}