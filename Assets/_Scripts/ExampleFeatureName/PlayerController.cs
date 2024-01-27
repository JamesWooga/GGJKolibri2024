using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.ExampleFeatureName
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _forceAmount;
        [SerializeField] private ForceMode2D _forceMode;
        [SerializeField] private Transform _rotationalAnchorPoint;
        [SerializeField] private float _maxMagnitude;
        [SerializeField] private float _maxRotate;
        
        private void FixedUpdate()
        {
            CalculateInput();
            ClampVelocity();
            UpdateRotationalAnchor();
        }

        private void CalculateInput()
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
        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity.normalized;
            if (_rigidbody.velocity.magnitude > _maxMagnitude)
            {
                _rigidbody.velocity = velocity * _maxMagnitude;
            }
        }

        private void UpdateRotationalAnchor()
        {
            _rotationalAnchorPoint.transform.position = _rigidbody.position;
            
            var horizontalVelocity = _rigidbody.velocity.x;
            var targetAngle = horizontalVelocity.Remap(-_maxMagnitude, _maxMagnitude, _maxRotate, -_maxRotate);

            _rotationalAnchorPoint.DORotate(new Vector3(0f, 0f, targetAngle), 0.01f, RotateMode.LocalAxisAdd);
        }
    }
}