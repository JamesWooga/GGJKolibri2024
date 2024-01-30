using _Scripts.GameState;
using _Scripts.Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility.Extensions;
using Debug = System.Diagnostics.Debug;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Rigidbody2D _bodyRigidbody;
        [SerializeField] private SpringJoint2D _spring;
        [SerializeField] private ForceMode2D _forceMode;
        [SerializeField] private Transform _rotationalAnchorPoint;
        [SerializeField] private PlayerCatchPoint _leftCatchPoint;
        [SerializeField] private PlayerCatchPoint _rightCatchPoint;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private LayerMask _floorLayer;
        [SerializeField] private float _floorCheckLength;
        [SerializeField] private InputActionReference _tiltLeft;
        [SerializeField] private InputActionReference _tiltRight;
        
        [Header("Tweakable Values (hover for description)")]
        
        [Header("Directly From Input")]
        [Tooltip("How much acceleration the wheel will have"), SerializeField] private float _wheelForceAmount;
        [Tooltip("How much acceleration the body rotation will have"), SerializeField] private float _bodyTorqueAmount;
        [Tooltip("Should the wheel have force applied when you hit A or D"), SerializeField] private bool _enableApplyingForceToWheel;
        [Tooltip("Apply wheel movement with W/S"), SerializeField] private bool _enableWSControlsForWheel;
        [Tooltip("How much rotational force should be applied to the body torque (in opposite direction of movement)"), SerializeField] private float _bodyTorqueAmountWithWsControls;

        [Header("Physics")]
        [Tooltip("The top speed for the wheel to be able to go"), SerializeField]  private float _maxWheelMagnitude;
        [Tooltip("How much the player leaning will affect the movement of the wheel"), SerializeField]  private float _leanForceAmount;
        [Tooltip("The top speed for the body to rotate at"), SerializeField]  private float _maxBodyTorque;
        [Tooltip("The top speed for the body to rotate at when in the air"), SerializeField]  private float _maxBodyTorqueInAir;
        [Tooltip("How rigid the movement between wheel and body should be. 0 is very elastic, 20 is very rigid"), SerializeField]  private float _bodyToWheelRigidity;
        [Tooltip("How fast can you rotate the body whilst in the air (easy backflips)"), SerializeField] private float _inAirBodyTorqueMultiplier;
        [Tooltip("When the game starts, between what values of velocity should be added to the player"), SerializeField] private Vector2 _initialVelocityAddedRange;
        [Tooltip("When the game starts, between what values of tilt should be added to the body"), SerializeField] private Vector2 _initialTiltAddedRange;
        [Tooltip("How fast should the player have to spin before yeeting"), SerializeField] private float _maxRotationalSpeedUntilGameOver;
        [Tooltip("Yeet amount"), SerializeField] private float _gameOverWheelForceApply;
        
        
        [Header("Objects")]
        [Tooltip("How much the objects on the catch points will affect the body rotating"), SerializeField]  private float _objectForcePerKg;
        [Tooltip("How much you fly in the air after an obstacle hits the rope"), SerializeField] private float _obstacleHitRopeWeightMultiplier;
        [Tooltip("Should the obstacles hitting the rope make you go (roughly) in the direction you are tilted"), SerializeField] private bool _shouldJumpInDirectionTilted;
        [Tooltip("How much should you fly directionally"), SerializeField] private float _directionalJumpForceMultiplier;
        [Tooltip("How rigid should the spring be in the air"), SerializeField] private float _bodyToWheelRigidityInAir;
        [Tooltip("What should your mass be in the air"), SerializeField] private float _rigidbodyMassInAir;

        public float MaxWheelMagnitude => _maxWheelMagnitude;
        public Rigidbody2D Rigidbody => _rigidbody;
        public float CurrentTilt { get; private set; }
        public float AllWeight => GetTotalScore();

        // [Header("Lose Conditions")] 
        // [SerializeField] private float _maxBodyAngleBeforeDeath;
        
        private bool _isGrounded;
        private bool _flownAtleastOnce;
        private int _firstChosenDirection;
        private bool _hasLost;
        
        private void Start()
        {
            _spring.frequency = _bodyToWheelRigidity;
            GameEvents.GameEvents.OnObstacleHitRope += HandleObstacleHitRope;
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }
        
        private void OnEnable()
        {
            _tiltLeft.action.Enable();
            _tiltRight.action.Enable();
        }

        private void OnDisable()
        {
            _tiltLeft.action.Disable();
            _tiltRight.action.Disable();
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.Play)
            {
                var value = _initialVelocityAddedRange.RandomBetweenXAndY();
                _rigidbody.AddForce(Vector2.right * (value * _firstChosenDirection), _forceMode);

                var tilt = _initialTiltAddedRange.RandomBetweenXAndY();
                _bodyRigidbody.AddTorque(tilt * _firstChosenDirection);
            }
        }

        private void OnDestroy()
        {
            GameEvents.GameEvents.OnObstacleHitRope -= HandleObstacleHitRope;
        }

        private void Update()
        {
            if (_firstChosenDirection == 0)
            {
                var isPressingLeft = Input.GetKey(KeyCode.A);
                var isPressingUp = Input.GetKey(KeyCode.W);
                
                var isPressingRight = Input.GetKey(KeyCode.D);
                var isPressingDown = Input.GetKey(KeyCode.S);

                if (isPressingLeft || isPressingUp)
                {
                    _firstChosenDirection = -1;
                }
                else if (isPressingRight || isPressingDown)
                {
                    _firstChosenDirection = 1;
                }
            }
            
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
            if (GameManager.Instance.GameState != GameState.GameState.Play || _hasLost || GameManager.Instance.IsInputBlocked)
            {
                return;
            }
            
            CalculateInput();
            if (_isGrounded)
            {
                ApplyLeanForce(); 
            }
            
            ClampAngularVelocity();
            CalculateObjectWeights();
            ClampVelocity();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Death"))
            {
                GameManager.Instance.SetScore(GetTotalScore());
                GameManager.Instance.SetGameState(GameState.GameState.GameOver);
                GameManager.Instance.EndRun();
            }
        }

        private void CalculateInput()
        {
            var isPressingLeft = _tiltLeft.action.IsPressed();
            var isPressingRight = _tiltRight.action.IsPressed();
            
            if (isPressingLeft)
            {
                var torque = _bodyTorqueAmount * (_isGrounded ? 1f : _inAirBodyTorqueMultiplier);
                _bodyRigidbody.AddTorque(torque);
                if (_isGrounded && _enableApplyingForceToWheel)
                {
                    _rigidbody.AddForce(Vector2.left * _wheelForceAmount, _forceMode);    
                }
            }
            
            if (isPressingRight)
            {
                var torque = -_bodyTorqueAmount * (_isGrounded ? 1f : _inAirBodyTorqueMultiplier);
                _bodyRigidbody.AddTorque(torque);
                if (_isGrounded && _enableApplyingForceToWheel)
                {
                    _rigidbody.AddForce(Vector2.right * _wheelForceAmount, _forceMode);
                }
            }

            if (_enableWSControlsForWheel)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    _rigidbody.AddForce(Vector2.left * _wheelForceAmount, _forceMode);
                    if (CurrentTilt > 0)
                    {
                        _bodyRigidbody.AddTorque(-_bodyTorqueAmountWithWsControls);
                    }
                }

                if (Input.GetKey(KeyCode.S))
                {
                    _rigidbody.AddForce(Vector2.right * _wheelForceAmount, _forceMode);
                    if (CurrentTilt < 0)
                    {
                        _bodyRigidbody.AddTorque(_bodyTorqueAmountWithWsControls);
                    }
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

        private void ClampAngularVelocity()
        {
            var maxValue = _isGrounded ? _maxBodyTorque : _maxBodyTorqueInAir;
            var sign = Mathf.Sign(_bodyRigidbody.angularVelocity);
            if (Mathf.Abs(_bodyRigidbody.angularVelocity) > maxValue)
            {
                _bodyRigidbody.angularVelocity = maxValue * sign;
            }
        }

        private void ApplyLeanForce()
        {
            var angle = _rotationalAnchorPoint.rotation.eulerAngles.z;
            var updated = Mathf.Repeat(angle + 180, 360) - 180;
            var lean = updated.Remap(-90f, 90f, -_leanForceAmount, _leanForceAmount);
            CurrentTilt = updated;
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

        private float GetTotalScore()
        {
            return _leftCatchPoint.TotalWeight + _rightCatchPoint.TotalWeight;
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
            _playerAnimator.Jump();
        }

        private void CheckDeathCondition()
        {
            var sign = Mathf.Sign(_bodyRigidbody.angularVelocity);
            var abs = Mathf.Abs(_bodyRigidbody.angularVelocity);
            if (abs < _maxRotationalSpeedUntilGameOver)
            {
                return;
            }

            _rigidbody.AddForce(Vector2.left * (sign * _gameOverWheelForceApply), ForceMode2D.Force);
            _hasLost = true;
        }
    }
}