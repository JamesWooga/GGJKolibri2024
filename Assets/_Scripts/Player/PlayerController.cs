using _Scripts.GameState;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Rigidbody2D _bodyRigidbody;
        [SerializeField] private ForceMode2D _forceMode;
        [SerializeField] private Transform _rotationalAnchorPoint;
        [SerializeField] private PlayerCatchPoint _leftCatchPoint;
        [SerializeField] private PlayerCatchPoint _rightCatchPoint;
        
        [Header("Tweakable Values (hover for description)")]
        
        [Header("Directly From Input")]
        [Tooltip("How much acceleration the wheel will have"), SerializeField] private float _wheelForceAmount;
        [Tooltip("How much acceleration the body rotation will have"), SerializeField] private float _bodyTorqueAmount;
        
        [Header("Rotation")]
        [Tooltip("The top speed for the wheel to be able to go"), SerializeField] private float _maxWheelMagnitude;
        [Tooltip("How much the player leaning will affect the movement of the wheel"), SerializeField] private float _leanForceAmount;
        [Tooltip("The top speed for the body to rotate at"), SerializeField] private float _maxBodyTorque;
        
        [Header("Objects")] 
        [Tooltip("How much the objects on the catch points will affect the wheel moving"), SerializeField] private float _objectForcePerKg;
        
        private void FixedUpdate()
        {
            CalculateInput();
            ApplyLeanForce();
            CalculateObjectWeights();
            ClampVelocity();
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
                _bodyRigidbody.AddTorque(_bodyTorqueAmount);
                _rigidbody.AddForce(Vector2.left * _wheelForceAmount, _forceMode);
            }
            
            if (isPressingRight)
            {
                _bodyRigidbody.AddTorque(-_bodyTorqueAmount);
                _rigidbody.AddForce(Vector2.right * _wheelForceAmount, _forceMode);
            }
        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity.normalized;
            if (_rigidbody.velocity.magnitude > _maxWheelMagnitude)
            {
                _rigidbody.velocity = velocity * _maxWheelMagnitude;
            }

            _bodyRigidbody.totalTorque = Mathf.Min(_maxBodyTorque, _bodyRigidbody.totalTorque);
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