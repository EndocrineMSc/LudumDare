using Audio;
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
        public UnityEvent HowToPlayOpened;

        private readonly string LEVEL_ONE_PARAM = "LevelOne";

        internal int Lives { get; set; } = 3;

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
            SwitchState(GameState.MainMenu);
            Application.targetFrameRate = 60;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        internal void SwitchState (GameState state)
        {
            State = state;

            switch (state)
            {
                case GameState.MainMenu:
                    AudioManager.Instance.FadeGameTrack(Track._001_Ambient_01, Fade.Out);
                    AudioManager.Instance.FadeGameTrack(Track._002_Ambient_02, Fade.Out);
                    MainMenuOpened?.Invoke();
                    break;
                case GameState.SettingsMenu:
                    SettingsOpened?.Invoke();
                    break;
                case GameState.CreditsMenu:
                    CreditsOpened?.Invoke();
                    break;
                case GameState.HowToPlayMenu:
                    HowToPlayOpened?.Invoke();
                    break;
                case GameState.LevelOne:
                    SceneManager.LoadSceneAsync(LEVEL_ONE_PARAM);
                    AudioManager.Instance.FadeGameTrack(Track._001_Ambient_01, Fade.In);
                    AudioManager.Instance.FadeGameTrack(Track._002_Ambient_02, Fade.In);
                    break;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains("MainMenu"))
            {
                Debug.Log("Main Menu Loaded");
                SwitchState(GameState.MainMenu);
                GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
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
        HowToPlayMenu,
        LevelOne,
    }
}
