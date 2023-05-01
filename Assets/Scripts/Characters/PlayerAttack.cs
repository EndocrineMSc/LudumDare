using System.Collections;
using UnityEngine;
using Utilities;

namespace Characters
{
    internal class PlayerAttack : MonoBehaviour
    {
        #region Fields and Properties

        //Shooting
        [SerializeField]private GameObject _shotPrefab;
        [SerializeField] private float _shotSpeed = 10f;
        
        //Aiming
        private GameObject _aimRecticle;
        private float _aimSpeed = 1.2f;
        private float _aimTimer = 0;
        internal bool KeepAiming;
        private bool _aimAborted;
        private GameObject _player;
        private bool _recticleIsOnWayUp = true;

        //Visuals
        private SpriteRenderer _recticleSprite;
        private bool _shotIsOnCooldown;
        private Animator _animator;

        #endregion

        #region Functions

        private void Awake()
        {
            _aimRecticle = transform.GetChild(0).gameObject;
            _player = transform.parent.gameObject;
            _recticleSprite = _aimRecticle.GetComponent<SpriteRenderer>();
            _animator = _player.GetComponent<Animator>();
        }

        private IEnumerator ResetAiming()
        {
            yield return new WaitForSeconds(1f);
            _aimAborted = false;
        }

        private void Update()
        {
            _recticleSprite.enabled = KeepAiming;

            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyUp(KeyCode.LeftAlt)) && !PauseControl.Instance.GameIsPaused && !_aimAborted)
            {
                _animator.SetTrigger("Aim");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                KeepAiming = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _animator.SetTrigger("Idle");
                _aimAborted = true;
                KeepAiming = false;
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                StartCoroutine(ResetAiming());
            }

            if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.LeftAlt)) && !PauseControl.Instance.GameIsPaused && !_aimAborted)
            {
                Shoot(GetShotDirection());
                transform.eulerAngles = new(0, 0, 0);
                _recticleIsOnWayUp = true;
                KeepAiming = false;
                _aimTimer = 0;
            }

            if (KeepAiming && !_shotIsOnCooldown)
            {
                float maxAngle = _player.transform.localScale.x > 0 ? 100 : -100;
                float angle = _recticleIsOnWayUp ? Mathf.Lerp(0, maxAngle, _aimTimer / _aimSpeed) : Mathf.Lerp(maxAngle, 0, _aimTimer /_aimSpeed);
                transform.eulerAngles = new(0, 0, angle);
                _aimTimer += Time.deltaTime;

                if (Mathf.Abs(angle) >= Mathf.Abs(maxAngle) && _recticleIsOnWayUp)
                {
                    _recticleIsOnWayUp = false;
                    transform.eulerAngles = _player.transform.localScale.x > 0 ? new(0, 0, 100) : new(0, 0, -100);
                    _aimTimer = 0;
                }

                if (Mathf.Abs(angle) < 0.01f && !_recticleIsOnWayUp)
                {
                    _recticleIsOnWayUp = true;
                    transform.eulerAngles = Vector3.zero;
                    _aimTimer = 0;
                }
            }

        }

        private Vector2 GetShotDirection()
        {
            return _aimRecticle.transform.position - transform.position;
        }

        private void Shoot(Vector2 direction)
        {
            _animator.SetTrigger("Attack");
            GameObject shot = Instantiate(_shotPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rigidbody = shot.GetComponent<Rigidbody2D>();
            rigidbody.velocity = direction * _shotSpeed;
        }
    
        #endregion
    }
}
