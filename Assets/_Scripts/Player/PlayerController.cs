using _Scripts.GameState;
using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _forceAmount;
        [SerializeField] private ForceMode2D _forceMode;
        
        [Header("Rotation")]
        [SerializeField] private Transform _rotationalAnchorPoint;
        [SerializeField] private float _maxMagnitude;
        [SerializeField] private float _maxRotate;
        [SerializeField] private float _leanForceAmount;
        
        private void FixedUpdate()
        {
            CalculateInput();
            ClampVelocity();
            UpdateRotationalAnchor();
            ApplyLeanForce();
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

            _rotationalAnchorPoint.DORotate(new Vector3(0f, 0f, targetAngle), 0.01f, RotateMode.LocalAxisAdd)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        private void ApplyLeanForce()
        {
            var angle = _rotationalAnchorPoint.rotation.eulerAngles.z;
            var updated = Mathf.Repeat(angle + 180, 360) - 180;
            var lean = updated.Remap(-90f, 90f, -_leanForceAmount, _leanForceAmount);
            _rigidbody.AddForce(new Vector2(-lean, 0f), _forceMode);
        }
    }
}