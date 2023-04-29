using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Utilities
{
    internal class GameManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static GameManager Instance { get; private set; }
        internal GameState State { get; private set; }

        public UnityEvent MainMenuOpened;
        public UnityEvent SettingsOpened;
        public UnityEvent CreditsOpened;

        private readonly string LEVEL_ONE_PARAM = "LevelOne";

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Physics2D.IgnoreLayerCollision(7, 8);
            SwitchState(GameState.MainMenu);
        }

        internal void SwitchState (GameState state)
        {
            State = state;

            switch (state)
            {
                case GameState.MainMenu:
                    MainMenuOpened.Invoke();
                    break;
                case GameState.SettingsMenu:
                    SettingsOpened.Invoke();
                    break;
                case GameState.CreditsMenu:
                    CreditsOpened.Invoke();
                    break;
                case GameState.LevelOne:
                    SceneManager.LoadSceneAsync(LEVEL_ONE_PARAM);
                    break;
            }
        }

        #endregion
    }

    internal enum GameState
    {
        MainMenu,
        SettingsMenu,
        CreditsMenu,
        PauseMenu,
        LevelOne,
    }
}
