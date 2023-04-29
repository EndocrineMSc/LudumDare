using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using TMPro;

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
        private bool _isAiming;
        private Ease _aimEase = Ease.Linear;
        private bool _keepAiming;
        private GameObject _player;

        #endregion

        #region Functions

        private void Awake()
        {
            _aimRecticle = transform.GetChild(0).gameObject;
            _player = transform.parent.gameObject;
        }

        private IEnumerator AimTweening()
        {
            if (!_isAiming)
            {
                _isAiming = true;
                if (_player.transform.localScale.x > 0)
                {
                    transform.DORotateQuaternion(Quaternion.Euler(0, 0, 100), _aimSpeed).SetEase(_aimEase);
                    yield return new WaitForSeconds(_aimSpeed);
                    transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), _aimSpeed).SetEase(_aimEase);
                    yield return new WaitForSeconds(_aimSpeed);
                }
                else
                {
                    transform.DORotateQuaternion(Quaternion.Euler(0, 0, -100), _aimSpeed).SetEase(_aimEase);
                    yield return new WaitForSeconds(_aimSpeed);
                    transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), _aimSpeed).SetEase(_aimEase);
                    yield return new WaitForSeconds(_aimSpeed);
                }
                _isAiming = false;
            }
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.F))
            {
                _keepAiming = true;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _keepAiming = false;
            }

            if(Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.F))
            {
                Shoot(GetShotDirection());
                _keepAiming = false;
            }

            if (_keepAiming)
            {
                StartCoroutine(AimTweening());
            }
        }

        private Vector2 GetShotDirection()
        {
            return _aimRecticle.transform.position - transform.position;
        }

        private void Shoot(Vector2 direction)
        {
            GameObject shot = Instantiate(_shotPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rigidbody = shot.GetComponent<Rigidbody2D>();
            rigidbody.velocity = direction * _shotSpeed;
        }



        #endregion
    }
}
