using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using Utilites;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Utilities
{
    internal class WinZone : MonoBehaviour
    {
        private bool _gameIsWon;
        private Animator _witchAnimator;
        [SerializeField] private Image _fadeImage;

        private void Start()
        { 
            _witchAnimator = GameObject.Find("Witch").GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_gameIsWon && collision.gameObject.CompareTag("Player"))
            {
                _gameIsWon = true;
                _witchAnimator.SetTrigger("Speak");
                LevelEvents.Instance.GameIsWon?.Invoke();
                Debug.Log("Level Won!");
                StartCoroutine(WaitThenLoadVictory());
            }
        }

        private IEnumerator WaitThenLoadVictory()
        {
            _fadeImage.DOFade(1, 2.9f);
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync("GameWin");
        }
    }
}