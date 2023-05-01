using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Utilites;
using Characters;
using UnityEngine.SceneManagement;

namespace Utilities
{
    internal class LifeCounterScreen : MonoBehaviour
    {
        #region Fields and Properties

        private Image _image;
        [SerializeField] private Image _lifeCounterImage;
        [SerializeField] private TextMeshProUGUI _lifeCounterGUI;

        private LifeCounter _lifeCounter;

        #endregion

        #region Functions

        private void Awake()
        {
            _image = GetComponent<Image>();
            _lifeCounter = GetComponentInChildren<LifeCounter>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _image.DOFade(0, 0.01f);
            _lifeCounterGUI.DOFade(0, 0.01f);
            _lifeCounterImage.DOFade(0, 0.01f);
            LevelEvents.Instance.LifeIsLost?.AddListener(OnLifeIsLost);
        }

        private void OnDisable()
        {
            LevelEvents.Instance.LifeIsLost?.RemoveListener(OnLifeIsLost);
        }

        private void OnLifeIsLost()
        {
            StartCoroutine(HandleFades());
        }

        private IEnumerator HandleFades()
        {
            if (GameManager.Instance.Lives > 0)
            {
                _image.DOFade(1, 1f);
                _lifeCounterImage.DOFade(1, 1f);
                yield return new WaitForSeconds(1);
                StartCoroutine(_lifeCounter.ChangeLifeCount());
                yield return new WaitForSeconds(1);
                SceneManager.LoadSceneAsync("LoadingScreen");
            }
            else
            {
                _image.DOFade(1, 1f);
                SceneManager.LoadSceneAsync("GameOver");
            }
        }

        #endregion
    }
}
