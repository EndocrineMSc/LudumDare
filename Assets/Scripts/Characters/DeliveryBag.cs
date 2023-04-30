using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

namespace Characters
{
    internal class DeliveryBag : MonoBehaviour
    {
        #region Fields and Properties

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private Player _player;
       
        internal int Countdown { get; private set; }
        private float _countdownHelper;
        private bool _countdownIsSet;

        [SerializeField] private float _shotSpeed;
        [SerializeField] private float _movementCooldown;
        [SerializeField] private float _maxUpperYDistance;
        [SerializeField] private float _maxLowerYDistance;
        [SerializeField] private float _maxXDistance;
        private float _movementMaxCooldown;


        #endregion

        #region Functions

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            StartCoroutine(HandleColliderAtAwake());
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.AddForce(new Vector2(-1, 1) * _shotSpeed, ForceMode2D.Impulse);
            _movementMaxCooldown = _movementCooldown;
        }

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        internal void SetCountdown(int countdown)
        {
            Countdown = countdown;
            _countdownHelper = countdown;
            _countdownIsSet = true;
        }

        void Update()
        {
            if (_countdownIsSet && Countdown > 0)
            {
                _countdownHelper -= Time.deltaTime;
                Countdown = Mathf.RoundToInt(_countdownHelper);
                _movementCooldown -= Time.deltaTime;

                if(_movementCooldown <= 0)
                {
                    float yDistanceToPlayer = transform.position.y - _player.transform.position.y;
                    float xDistanceToPlayer = transform.position.x - _player.transform.position.x;

                    if (yDistanceToPlayer < -_maxLowerYDistance
                        || yDistanceToPlayer > _maxUpperYDistance
                        || Mathf.Abs(xDistanceToPlayer) > _maxXDistance)
                    {
                        Vector2 direction;

                        if (yDistanceToPlayer < -_maxLowerYDistance)
                        {
                            direction = Vector2.up;
                            _rigidbody.AddForce(direction * _shotSpeed, ForceMode2D.Impulse);
                        }
                        else if (yDistanceToPlayer > _maxUpperYDistance)
                        {
                            direction = Vector2.down;
                            _rigidbody.AddForce(direction * _shotSpeed, ForceMode2D.Impulse);
                        }                                            

                        if (Mathf.Abs(xDistanceToPlayer) > _maxXDistance)
                        {
                            if (xDistanceToPlayer < 0)
                            {
                                direction = Vector2.right;
                            }
                            else
                            {
                                direction = Vector2.left;
                            }
                            _rigidbody.AddForce(direction * _shotSpeed, ForceMode2D.Impulse);
                        }
                    }
                    else
                    {
                        MoveInRandomDirection();
                    }
                    _movementCooldown = _movementMaxCooldown;
                }
            }       
        }
        
        private IEnumerator HandleColliderAtAwake()
        {
            Physics2D.IgnoreLayerCollision(8, 9, true);
            yield return new WaitForSeconds(1f);
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }

        private void MoveInRandomDirection()
        {
            float randomX = UnityEngine.Random.Range(-1, 1);
            float randomY = UnityEngine.Random.Range(-1, 1);

            _rigidbody.AddForce(new Vector2(randomX,randomY) * _shotSpeed, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Shot>(out _))
            {
                _rigidbody.AddForce(Vector2.down * _shotSpeed, ForceMode2D.Impulse);
            }
        }

        #endregion
    }
}
