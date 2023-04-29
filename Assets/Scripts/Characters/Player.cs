using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Utilites;

namespace Characters
{
    internal class Player : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        private readonly string HURT_TRIGGER = "Hurt";
        private readonly string WALK_TRIGGER = "Walk";
        private readonly string IDLE_TRIGGER = "Idle";
        private readonly string ATTACK_TRIGGER = "Attack";
        private readonly string JUMP_TRIGGER = "Jump";
        private readonly string GROUND_LAYER = "Ground";
        private readonly string X_AXIS = "Horizontal";
        private readonly string Y_AXIS = "Vertical";

        private float _hurtAnimationDuration = 1f;
        private Animator _playerAnimator;
        internal bool IsInHurtAnimation { get; private set; }

        [SerializeField] private GameObject _deliverySackPrefab;
        private bool _isHoldingSack = true;

        #region Movement Fields

        //Player
        private Rigidbody2D _rigidbody;
        private Collider2D _playerCollider;
        [SerializeField] private LayerMask _groundLayer;
        private Vector2 _moveInput;
        private GameObject _playerVisuals;
        
        private bool _isFacingRight;
        private bool _isFacingLeft;
        private bool _isJumping;
        private bool _isJumpFalling;
        private bool _isJumpCut;
        private bool _isFalling;

        private float _lastOnGroundTime;
        private float _lastPressedJumpTime;

        [Header("Gravity")]
        private float _gravityStrength;
        private float _gravityScale;
        [SerializeField] private float _maxFallSpeed;
        [SerializeField] private float _fallGravityMultiplier;
        [SerializeField] private float _fastFallGravityMultiplier;
        [SerializeField] private float _maxFastFallSpeed;

        [Header("Run")]
        [SerializeField] private float _runMaxSpeed;
        [SerializeField] private float _runAccelerationSpeed;
        private float _runAccelerationAmount;
        [SerializeField] private float _runDeccelerationSpeed;
        private float _runDeccelerationAmount;
        [SerializeField] private float _accelarationInAir;
        [SerializeField] private float _deccelerationInAir;
        private bool _isConservingMomentum = true;

        [Header("Jump")]
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpTimeToApex;
        private float _jumpForce;
        [SerializeField] private float _jumpCutGravityMultiplier;
        [SerializeField, Range(0f,1f)] private float _jumpHangGravityMultiplier;
        [SerializeField] private float _jumpHangTimeThreshold;
        [SerializeField] private float _jumpHangAccelerationMultiplier;
        [SerializeField] private float _jumpHangMaxSpeedMultiplier;
        [SerializeField, Range(0.01f, 0.5f)] private float _coyoteTime;
        [SerializeField, Range(0.01f, 0.5f)] private float _jumpInputBufferTime;
        private Vector2 _groundCheckSize;
        private Vector2 _groundCheckPoint;

        #endregion
        #endregion

        #region Functions

        private void Awake()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerCollider = GetComponentInChildren<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerVisuals = transform.GetChild(1).gameObject;
        }

        private void Start()
        {
            LevelEvents.Instance.LifeIsLost?.AddListener(OnLifeIsLost);
            SetGravityScale(_gravityScale);
            _isFacingRight = true;
            _groundCheckSize = new(_playerCollider.bounds.size.x - 0.05f, _playerCollider.bounds.size.y);
        }

        private void OnValidate()
        {
            _gravityStrength = -(2 * _jumpHeight) / (_jumpTimeToApex * _jumpTimeToApex);
            _gravityScale = _gravityStrength / Physics2D.gravity.y;
            _runAccelerationAmount = (50 * _runAccelerationSpeed) / _runMaxSpeed;
            _runDeccelerationAmount = (50 * _runDeccelerationSpeed) / _runMaxSpeed;
            _jumpForce = Mathf.Abs(_gravityStrength) * _jumpTimeToApex;

            _runAccelerationSpeed = Mathf.Clamp(_runAccelerationSpeed, 0.01f, _runMaxSpeed);
            _runDeccelerationSpeed = Mathf.Clamp(_runDeccelerationSpeed, 0.01f, _runMaxSpeed);           
        }

        private void SetGravityScale(float gravityScale)
        {
            _rigidbody.gravityScale = gravityScale;
        }

        private void Update()
        {
            //Timers
            _lastOnGroundTime -= Time.deltaTime;
            _lastPressedJumpTime -= Time.deltaTime;

            //Inputs
            if (!IsInHurtAnimation)
            {
                _moveInput.x = Input.GetAxisRaw(X_AXIS);
                _moveInput.y = Input.GetAxisRaw(Y_AXIS);
            }

            if (_moveInput.x != 0)
            {
                CheckDirectionToFace(_moveInput.x > 0);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J))
            {
                OnJumpInput();
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.J))
            {
                OnJumpUpInput();
            }

            //Collision Checks
            _groundCheckPoint = new(_playerVisuals.transform.position.x, _playerVisuals.transform.position.y - 0.2f);

            if(IsGrounded() && !_isJumping)
            {
                _lastOnGroundTime = _coyoteTime;
            }

            //Jump Checks
            if(_isJumping && _rigidbody.velocity.y < 0)
            {
                _isJumping = false;
                _isJumpFalling = true;
            }

            if(_lastOnGroundTime > 0 && !_isJumping)
            {
                _isJumpCut = false;

                if(!_isJumping)
                {
                    _isJumpFalling = false;
                }
            }

            //Jump
            if(CanJump() && _lastPressedJumpTime > 0)
            {
                _isJumping = true;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }

            //Gravity
            if (_rigidbody.velocity.y < 0 && _moveInput.y <0)
            {
                SetGravityScale(_gravityScale * _fastFallGravityMultiplier);
                _rigidbody.velocity = new(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_maxFastFallSpeed));
            }
            else if(_isJumpCut)
            {
                SetGravityScale(_gravityScale * _jumpCutGravityMultiplier);
                _rigidbody.velocity = new(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_maxFastFallSpeed));
            }
            else if((_isJumping || _isJumpFalling) && Mathf.Abs(_rigidbody.velocity.y) < _jumpHangTimeThreshold)
            {
                SetGravityScale(_gravityScale * _jumpCutGravityMultiplier);
            }
            else if (_rigidbody.velocity.y < 0)
            {
                SetGravityScale(_gravityScale * _fallGravityMultiplier);
            }
            else
            {
                SetGravityScale(_gravityScale);
            }
        }

        private void FixedUpdate()
        {
            Run(1);
        }


        private void OnJumpInput()
        {
            _lastPressedJumpTime = _jumpInputBufferTime;
        }

        private void OnJumpUpInput()
        {
            if (CanJumpCut())
            {
                _isJumpCut = true;
            }
        }

        private void Run(float lerpAmount)
        {
            float targetSpeed = _moveInput.x * _runMaxSpeed;
            targetSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, lerpAmount);

            //Calculate AccelerationRate
            float accelerationRate;
            if(_lastOnGroundTime > 0)
            {
                accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _runAccelerationAmount : _runDeccelerationAmount;
            }
            else
            {
                accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _runAccelerationAmount * _accelarationInAir : _runDeccelerationAmount * _deccelerationInAir;
            }

            //Add Bonuds Jump Apex Acceleration
            if(_isJumping || _isJumpFalling)
            {
                accelerationRate *= _jumpHangAccelerationMultiplier;
                targetSpeed *= _jumpHangMaxSpeedMultiplier;
            }

            //Conserve Momentum
            if((Mathf.Abs(_rigidbody.velocity.x) > Mathf.Abs(targetSpeed)) 
                && (Mathf.Sign(_rigidbody.velocity.x) == Mathf.Sign(targetSpeed)) 
                && (Mathf.Abs(targetSpeed) > 0.01f) && (_lastOnGroundTime < 0))
            {
                accelerationRate = 0;
            }

            float speedDifference = targetSpeed - _rigidbody.velocity.x;
            float movement = speedDifference * accelerationRate;

            _rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void Turn()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            _isFacingRight = !_isFacingRight;
        }

        private void Jump()
        {
            _lastPressedJumpTime = 0;
            _lastOnGroundTime = 0;

            float force = _jumpForce;
            if (_rigidbody.velocity.y < 0)
            {
                force -= _rigidbody.velocity.y;
            }

            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        private void CheckDirectionToFace(bool isMovingRight) 
        {
            if(isMovingRight != _isFacingRight)
            {
                Turn();
            }
        }

        private bool CanJump()
        {
            if (_lastOnGroundTime > 0 && !_isJumping)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanJumpCut()
        {
            return _isJumping && _rigidbody.velocity.y > 0;
        }

        public void TakeDamage()
        {
            if(!IsInHurtAnimation)
            {
                StartCoroutine(HandleDamage());
            }                       
        }

        private IEnumerator HandleDamage()
        {
            IsInHurtAnimation = true;
            _playerAnimator.SetTrigger(HURT_TRIGGER);
            
            if (_isHoldingSack) 
            {
                Instantiate(_deliverySackPrefab, transform);
            }

            yield return new WaitForSeconds(_hurtAnimationDuration);
            IsInHurtAnimation = false;
        }

        private void OnLifeIsLost()
        {
            StartCoroutine(HandleLostLife());
        }

        private IEnumerator HandleLostLife()
        {
            IsInHurtAnimation = true;
            _playerAnimator.SetTrigger(HURT_TRIGGER);
            yield return new WaitForSeconds(_hurtAnimationDuration);
            LevelEvents.Instance.PlayerIsDestroyed?.Invoke();
            Destroy(gameObject);
        }

        private bool IsGrounded()
        {
            bool isGrounded = Physics2D.OverlapBox(_groundCheckPoint, _groundCheckSize, 0f, _groundLayer);
            return isGrounded;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheckPoint, _groundCheckSize);
        }

        #endregion
    }
}
