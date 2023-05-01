using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using Utilites;

namespace Utilities
{
    internal class WinZone : MonoBehaviour
    {
        private bool _gameIsWon;
        private Animator _witchAnimator;

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
            }

        }
    }
}