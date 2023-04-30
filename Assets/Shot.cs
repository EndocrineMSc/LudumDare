using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class Shot : MonoBehaviour
    {
        #region Fields and Properties

        private int _collisionCounter = 0;
        [SerializeField] private int _maxWallCollisions = 3;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        [SerializeField] private float _deathJumpHeight;
        [SerializeField] private float _deathJumpDistance;
        [SerializeField] private float _deathJumpForce;
        [SerializeField] private float _deathJumpGravity;

        #endregion

        #region Functions

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }   

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage();
                DeathTween();
                StartCoroutine(KillObject());
            }
            else
            {
                HandleCollision();
            }
        }

        private void HandleCollision()
        {
            _collisionCounter++;
            if (_collisionCounter > _maxWallCollisions)
            {
                _collider.enabled = false;
                _rigidbody.velocity = Vector3.zero;
                DeathTween();
                StartCoroutine(KillObject());
            }
        }

        private void DeathTween()
        {
            _rigidbody.AddForce(new Vector2(_deathJumpDistance, _deathJumpHeight) * _deathJumpForce);
            _rigidbody.gravityScale = _deathJumpGravity;
        }

        private IEnumerator KillObject()
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }

        #endregion


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
