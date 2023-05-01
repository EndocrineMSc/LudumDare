using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilites;
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

        [SerializeField] private float _shotSpeed;
        [SerializeField] private float _movementCooldown;
        [SerializeField] private float _maxUpperYDistance;
        [SerializeField] private float _maxLowerYDistance;
        [SerializeField] private float _maxXDistance;
        private float _movementMaxCooldown;
        private bool _isRetrievedByBird;

        private GameObject _birdSpawn;
        private bool _birdIsSpawned;

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
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _countdownHelper = _player.DeliveryBagTimer;
            Countdown = _player.DeliveryBagTimer;
            _birdSpawn = GameObject.FindGameObjectWithTag("BirdSpawn");
            LevelEvents.Instance.TimePowerUpPickedUp?.AddListener(OnPowerUp);
        }

        private void OnDisable()
        {
            LevelEvents.Instance.TimePowerUpPickedUp?.RemoveListener(OnPowerUp);
        }

        private void OnPowerUp()
        {
            _countdownHelper = _player.DeliveryBagTimer;
            Countdown = _player.DeliveryBagTimer;
        }

        void Update()
        {
            if (Countdown > 0 && !_isRetrievedByBird)
            {
                _countdownHelper -= Time.deltaTime;
                _player.DeliveryBagTimer = Mathf.RoundToInt(_countdownHelper);
                Countdown = _player.DeliveryBagTimer;
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
            
            if (Countdown == 0 && !_birdIsSpawned)
            {
                _birdIsSpawned = true;
                Instantiate(_player.CarrierBird, _birdSpawn.transform.position, Quaternion.identity);
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<CarrierBird>(out _))
            {
                _rigidbody.velocity = Vector2.zero;
                _isRetrievedByBird = true;
            }

            if (collision.gameObject.TryGetComponent<Shot>(out _))
            {
                _rigidbody.AddForce(Vector2.down * _shotSpeed, ForceMode2D.Impulse);
            }
        }

        #endregion
    }
}
