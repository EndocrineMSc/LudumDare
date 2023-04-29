using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilites;

namespace Characters
{
    internal class PlayerManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static PlayerManager Instance { get; private set; }

        [SerializeField] private GameObject _playerPrefab;
        private GameObject _playerSpawnPoint;
        private readonly string PLAYER_SPAWN_PARAM = "PlayerSpawn";

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject); // Destroy the duplicate
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _playerSpawnPoint = GameObject.FindGameObjectWithTag(PLAYER_SPAWN_PARAM);
            LevelEvents.Instance.PlayerIsDestroyed?.AddListener(OnPlayerDeath);
        }
        private void OnPlayerDeath()
        {
            Instantiate(_playerPrefab, _playerSpawnPoint.transform);
            //ToDo: Affix Camera to new Player
        }

        #endregion
    }
}
