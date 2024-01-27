using _Scripts.GameState;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Rigidbody2D _bodyRigidbody;
        [SerializeField] private float _forceAmount;
        [SerializeField] private ForceMode2D _forceMode;
        [SerializeField] private float _torqueAmount;
        
        [Header("Rotation")]
        [SerializeField] private Transform _rotationalAnchorPoint;
        [SerializeField] private float _maxMagnitude;
        [SerializeField] private float _leanForceAmount;
        
        [Header("Objects")] 
        [SerializeField] private PlayerCatchPoint _leftCatchPoint;
        [SerializeField] private PlayerCatchPoint _rightCatchPoint;
        [SerializeField] private float _objectForcePerKg;
        
        private void FixedUpdate()
        {
            CalculateInput();
            ApplyLeanForce();
            ClampVelocity();
            CalculateObjectWeights();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Death"))
            {
                GameStateManager.SetGameState(GameState.GameState.Defeat);
            }
        }

        private void CalculateInput()
        {
            var isPressingLeft = Input.GetKey(KeyCode.A);
            var isPressingRight = Input.GetKey(KeyCode.D);
            
            if (isPressingLeft)
            {
                _bodyRigidbody.AddTorque(_torqueAmount);
                _rigidbody.AddForce(Vector2.left * _forceAmount, _forceMode);
            }
            
            if (isPressingRight)
            {
                _bodyRigidbody.AddTorque(-_torqueAmount);
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

        private void ApplyLeanForce()
        {
            var angle = _rotationalAnchorPoint.rotation.eulerAngles.z;
            var updated = Mathf.Repeat(angle + 180, 360) - 180;
            var lean = updated.Remap(-90f, 90f, -_leanForceAmount, _leanForceAmount);
            _rigidbody.AddForce(new Vector2(-lean, 0f), _forceMode);
        }

        private void CalculateObjectWeights()
        {
            if (_leftCatchPoint == null || _rightCatchPoint == null)
            {
                return;
            }
            
            var leftForce = _leftCatchPoint.TotalWeight * _objectForcePerKg;
            var rightForce = _rightCatchPoint.TotalWeight * _objectForcePerKg;
            
            _rigidbody.AddForce(Vector2.left * leftForce, _forceMode);
            _rigidbody.AddForce(Vector2.right * rightForce, _forceMode);
        }
    }
}