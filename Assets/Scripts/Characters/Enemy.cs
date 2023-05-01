using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class Enemy : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Collider2D _colliderGroundFeeler;
        private SpriteRenderer _spriteRenderer;
        private EnemyTurnFeeler _turnFeeler;
        [SerializeField] private float _moveSpeed = 2;
        [SerializeField] private float _xJumpOffset;
        [SerializeField] private float yJumpOffset;
        [SerializeField] private float _jumpPower;
        [SerializeField] private float _jumpDuration;
        private bool _isDead;

        #endregion

        #region Functions

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody.freezeRotation = true;
            _colliderGroundFeeler = transform.GetChild(0).GetComponent<Collider2D>();
            _turnFeeler = transform.GetChild(0).GetComponent<EnemyTurnFeeler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody.velocity = Vector2.left * _moveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Shot>(out _))
            {
                TakeDamage();
            }
        }

        public void TakeDamage()
        {
            _collider.enabled = false;
            _colliderGroundFeeler.enabled = false;
            _rigidbody.gravityScale = 0;
            _rigidbody.freezeRotation = false;
            _isDead = true;
            _rigidbody.velocity = Vector2.zero;
            transform.DOJump(new Vector2(transform.position.x + _xJumpOffset, transform.position.y + yJumpOffset), _jumpPower, 1, _jumpDuration);
            transform.DORotateQuaternion(Quaternion.Euler(0, 0, 180), 0.2f);
        }

        internal void Turn()
        {
            if (_rigidbody.velocity.x > 0)
            {
                _rigidbody.velocity = Vector2.left * _moveSpeed;
                _spriteRenderer.flipX = false;
                _turnFeeler.GetComponent<SpriteRenderer>().flipX = false; 
                _turnFeeler.transform.localPosition = new(-_turnFeeler.transform.localPosition.x, _turnFeeler.transform.localPosition.y, _turnFeeler.transform.localPosition.z);
            }
            else
            {
                _rigidbody.velocity = Vector2.right * _moveSpeed;
                _spriteRenderer.flipX = true;
                _turnFeeler.GetComponent<SpriteRenderer>().flipX = true;
                _turnFeeler.transform.localPosition = new(-_turnFeeler.transform.localPosition.x, _turnFeeler.transform.localPosition.y, _turnFeeler.transform.localPosition.z);
            }
        }

        void Update()
        {
            if (!_isDead)
            {
                if (!_spriteRenderer.flipX)
                {
                    _rigidbody.velocity = Vector2.left * _moveSpeed;
                }
                else
                {
                    _rigidbody.velocity = Vector2.right * _moveSpeed;
                }
            }
        }

        #endregion
    }
}
