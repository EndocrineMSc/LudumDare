using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using TMPro;
using Utilites;
using UnityEngine.UI;
using DG.Tweening;

namespace Utilities
{
    internal class CountdownTimer : MonoBehaviour
    {
        private TextMeshProUGUI _countdownTimer;
        private bool _readFromPlayer = true;
        private bool _readFromBag;
        private Image _counterBackground;
        private bool _isActive;
        private bool _isPunching;
        private bool _shouldPunch;
        private Vector3 _punchVector;
        private int _currentCountdown;

        // Start is called before the first frame update
        private void Start()
        {
            _countdownTimer = GetComponentInChildren<TextMeshProUGUI>();
            _counterBackground = GetComponent<Image>();
            LevelEvents.Instance.BagIsLost?.AddListener(OnBagLost);
            LevelEvents.Instance.BagIsRetrieved?.AddListener(OnBagRetrieved);
            StartCoroutine(InactivateAfterDelay());
            _punchVector = _counterBackground.rectTransform.localScale * 1.5f;
        }

        private void OnDisable()
        {
            LevelEvents.Instance.BagIsLost?.RemoveListener(OnBagLost);
            LevelEvents.Instance.BagIsRetrieved?.RemoveListener(OnBagRetrieved);
        }

        // Update is called once per frame
        void Update()
        {
            if(_readFromPlayer && _isActive)
            {
                _currentCountdown = FindObjectOfType<Player>().DeliveryBagTimer;
                _countdownTimer.text = _currentCountdown.ToString();
            }
            else if(_readFromBag && _isActive)
            {
                _currentCountdown = FindObjectOfType<DeliveryBag>().Countdown;
                _countdownTimer.text = _currentCountdown.ToString();
            }

            if (_shouldPunch)
            {
                StartCoroutine(PunchThatImage());
            }
        }

        private void OnBagLost()
        {
            Activate();
            StartCoroutine(PunchThatImage());
            _shouldPunch = true;
            _readFromPlayer = false;
            _readFromBag = true;
        }

        private void OnBagRetrieved()
        {
            StartCoroutine(InactivateAfterDelay());
            _readFromPlayer = true;
            _readFromBag = false;
        }

        private IEnumerator InactivateAfterDelay()
        {
            _shouldPunch = false;
            yield return new WaitForSeconds(3);
            _counterBackground.DOFade(0, 1f);
            _countdownTimer.DOFade(0, 1f);
            _isActive = false;
        }

        private void Activate()
        {
            _isActive = true;
            _counterBackground.DOFade(1, 1f);
            _countdownTimer.DOFade(1, 1f);
        }

        private IEnumerator PunchThatImage()
        {
            if (!_isPunching && _currentCountdown > 5)
            {
                _isPunching = true;
                _counterBackground.rectTransform.DOPunchScale(_punchVector, 1f, 1);
                yield return new WaitForSeconds(1);
                _isPunching = false;
            }
            else if(!_isPunching && _currentCountdown <= 5)
            {
                _isPunching = true;
                _counterBackground.rectTransform.DOPunchScale(_punchVector * 1.5f, 1f, 2);
                yield return new WaitForSeconds(1);
                _isPunching = false;
            }
        }
    }
}
