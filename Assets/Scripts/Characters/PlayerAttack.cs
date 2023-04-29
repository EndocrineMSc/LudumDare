using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Characters
{
    internal class PlayerAttack : MonoBehaviour
    {
        #region Fields and Properties

        private GameObject _radiusTemplate;
        private GameObject _aimRecticle;
        private Tween _aimUpTween;
        private Tween _aimDownTween;
        private Vector2 _aimUpperEnd = new(0.2f, 0.35f);
        private float _aimSpeed = 1.2f;
        private bool _isAiming;
        private Ease _aimEase = Ease.Linear;
        private bool _keepAiming;

        #endregion

        #region Functions

        private void Awake()
        {
            _radiusTemplate = transform.GetChild(0).gameObject;
            _aimRecticle = _radiusTemplate.transform.GetChild(0).gameObject;
        }

        private IEnumerator AimTweening()
        {
            if (!_isAiming)
            {
                _isAiming = true;
                _radiusTemplate.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 100), _aimSpeed).SetEase(_aimEase);
                yield return new WaitForSeconds(_aimSpeed);
                _radiusTemplate.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), _aimSpeed).SetEase(_aimEase);
                yield return new WaitForSeconds(_aimSpeed);
                _isAiming = false;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _keepAiming = true;
            }

            if (_keepAiming)
            {
                StartCoroutine(AimTweening());
            }

            if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                _keepAiming = false;
            }
        }

        #endregion
    }
}
