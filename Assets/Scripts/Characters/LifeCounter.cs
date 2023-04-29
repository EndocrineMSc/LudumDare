using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Characters
{
    internal class LifeCounter : MonoBehaviour
    {
        #region Fields and Properties

        internal int LifeCount { get; private set; } = 3;

        private TextMeshProUGUI _lifeCounter;

        #endregion

        #region Functions

        private void Awake()
        {
            _lifeCounter = GetComponent<TextMeshProUGUI>();
            _lifeCounter.text = LifeCount.ToString();
        }

        public void DecreaseLifeCount()
        {
            LifeCount--;
            StartCoroutine(ChangeLifeCount());
        }

        internal void IncreaseLifeCount()
        {
            LifeCount++;
            StartCoroutine(ChangeLifeCount());
        }

        private IEnumerator ChangeLifeCount()
        {
            _lifeCounter.DOFade(0, 0.25f).SetEase(Ease.InOutBounce);
            yield return new WaitForSeconds(0.25f);
            _lifeCounter.DOFade(1, 0.25f).SetEase(Ease.InOutBounce);
            _lifeCounter.text = LifeCount.ToString();
        }

        #endregion
    }
}
