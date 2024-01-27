using _Scripts.GameState;
using _Scripts.Objects;
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
        [SerializeField] private LayerMask _floorLayer;
        [SerializeField] private float _floorCheckLength;
        
        [Header("Tweakable Values (hover for description)")]
        
        [Header("Directly From Input")]
        [Tooltip("How much acceleration the wheel will have"), SerializeField] private float _wheelForceAmount;
        [Tooltip("How much acceleration the body rotation will have"), SerializeField] private float _bodyTorqueAmount;

        [Header("Physics")]
        [Tooltip("The top speed for the wheel to be able to go"), SerializeField]  private float _maxWheelMagnitude;

        [Tooltip("How much the player leaning will affect the movement of the wheel"), SerializeField]  private float _leanForceAmount;
        [Tooltip("The top speed for the body to rotate at"), SerializeField]  private float _maxBodyTorque;
        [Tooltip("How rigid the movement between wheel and body should be. 0 is very elastic, 20 is very rigid"), SerializeField]  private float _bodyToWheelRigidity;
        [Tooltip("How fast can you rotate the body whilst in the air (easy backflips)"), SerializeField] private float _inAirBodyTorqueMultiplier;

        [Header("Objects")]
        [Tooltip("How much the objects on the catch points will affect the body rotating"), SerializeField]  private float _objectForcePerKg;
        [Tooltip("How much you fly in the air after an obstacle hits the rope"), SerializeField] private float _obstacleHitRopeWeightMultiplier;
        [Tooltip("Should the obstacles hitting the rope make you go (roughly) in the direction you are tilted"), SerializeField] private bool _shouldJumpInDirectionTilted;
        [Tooltip("How much should you fly directionally"), SerializeField] private float _directionalJumpForceMultiplier;
        [Tooltip("How rigid should the spring be in the air"), SerializeField] private float _bodyToWheelRigidityInAir;
        [Tooltip("What should your mass be in the air"), SerializeField] private float _rigidbodyMassInAir;
        
        // [Header("Lose Conditions")] 
        // [SerializeField] private float _maxBodyAngleBeforeDeath;
        
        private bool _isGrounded;
        private bool _flownAtleastOnce;
        private SpringJoint2D _spring;
        
        private void Awake()
        {
            _spring = _rigidbody.GetComponent<SpringJoint2D>();
            _spring.frequency = _bodyToWheelRigidity;
            GameStateManager.SetGameState(GameState.GameState.Play);
            GameEvents.GameEvents.OnObstacleHitRope += HandleObstacleHitRope;
        }

        private void OnDestroy()
        {
            GameEvents.GameEvents.OnObstacleHitRope -= HandleObstacleHitRope;
        }

        private void Update()
        {
            if (_flownAtleastOnce)
            {
                _spring.frequency = _isGrounded ? _bodyToWheelRigidity : _bodyToWheelRigidityInAir;
            }

            if (!_isGrounded && _flownAtleastOnce && _rigidbody.velocity.y <= 0)
            {
                var hit = Physics2D.Raycast(_rigidbody.position, Vector2.down, _floorCheckLength, _floorLayer);
                if (hit.collider != null)
                {
                    _rigidbody.mass = 1.0f;
                }    
            }

            CheckDeathCondition();
        }

        private void FixedUpdate()
        {
            if (GameStateManager.GameState != GameState.GameState.Play)
            {
                return;
            }
            
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
                var torque = _bodyTorqueAmount * (_isGrounded ? 1f : _inAirBodyTorqueMultiplier);
                _bodyRigidbody.AddTorque(torque);
                if (_isGrounded)
                {
                    _rigidbody.AddForce(Vector2.left * _wheelForceAmount, _forceMode);    
                }
            }
            
            if (isPressingRight)
            {
                var torque = -_bodyTorqueAmount * (_isGrounded ? 1f : _inAirBodyTorqueMultiplier);
                _bodyRigidbody.AddTorque(torque);
                if (_isGrounded)
                {
                    _rigidbody.AddForce(Vector2.right * _wheelForceAmount, _forceMode);
                }
            }
        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity.normalized;
            if (_rigidbody.velocity.magnitude > _maxWheelMagnitude)
            {
                _rigidbody.velocity = velocity * _maxWheelMagnitude;
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
            
            _bodyRigidbody.AddTorque(leftForce);
            _bodyRigidbody.AddTorque(-rightForce);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (CollisionWithGround(other))
            {
                _isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (CollisionWithGround(other))
            {
                _isGrounded = false;
                _flownAtleastOnce = true;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (CollisionWithGround(other))
            {
                _isGrounded = true;
            }
        }

        private static bool CollisionWithGround(Collision2D other)
        {
            return other.gameObject.CompareTag("Rope");
        }

        private void HandleObstacleHitRope(DroppedObject droppedObject)
        {
            if (!_isGrounded)
            {
                return;
            }
            
            // Calculate the jump force based on the direction and weight of the dropped object
            var jumpForce = Vector2.up * (droppedObject.Weight * _obstacleHitRopeWeightMultiplier);

            if (_shouldJumpInDirectionTilted)
            {
                // Adjust the jump force based on the rotation of the rotational anchor point
                var angle = _rotationalAnchorPoint.rotation.eulerAngles.z;
                var normalized = Mathf.Repeat(angle + 180, 360) - 180;
                normalized = normalized.Remap(-90f, 90f, 1f, -1f);
                jumpForce.x *= normalized * _directionalJumpForceMultiplier;
            }

            // Apply the jump force to the Rigidbody
            _rigidbody.mass = _rigidbodyMassInAir;
            _rigidbody.AddForce(jumpForce, ForceMode2D.Force);
        }

        private void CheckDeathCondition()
        {
            if (!_isGrounded)
            {
                return;
            }
            
            // In case we want to die when rotation gets too much
            // var angle = _rotationalAnchorPoint.rotation.eulerAngles.z;
            // var updated = Mathf.Repeat(angle + 180, 360) - 180;
            // var signed = Mathf.Abs(updated);
            // if (signed > _maxBodyAngleBeforeDeath)
            // {
            //     GameStateManager.SetGameState(GameState.GameState.Defeat);
            // }
        }
    }
}