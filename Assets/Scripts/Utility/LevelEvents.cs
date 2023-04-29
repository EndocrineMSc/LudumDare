using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utilites
{
    internal class LevelEvents : MonoBehaviour
    {
        internal static LevelEvents Instance { get; private set; }

        public UnityEvent GameIsOver;
        public UnityEvent GameIsWon;
        public UnityEvent LifeIsLost;
        public UnityEvent PlayerIsDestroyed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
