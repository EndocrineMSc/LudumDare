using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

namespace Characters
{
    internal class LifeCounter : MonoBehaviour
    {
        #region Fields and Properties

        private int _lifeCount;

        private TextMeshProUGUI _lifeCounter;

        #endregion

        #region Functions

        private void Awake()
        {
            _lifeCounter = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _lifeCount = GameManager.Instance.Lives;
            _lifeCounter.text = _lifeCount.ToString();
        }

        internal IEnumerator ChangeLifeCount()
        {
            _lifeCounter.DOFade(1, 0.25f).SetEase(Ease.InOutBounce);
            GameManager.Instance.Lives--;
            _lifeCount = GameManager.Instance.Lives;
            yield return new WaitForSeconds(0.25f);
            _lifeCounter.DOFade(0, 0.25f).SetEase(Ease.InOutBounce);
            yield return new WaitForSeconds(0.25f);
            _lifeCounter.DOFade(1, 0.25f).SetEase(Ease.InOutBounce);
            _lifeCounter.text = _lifeCount.ToString();
        }

        #endregion
    }
}
