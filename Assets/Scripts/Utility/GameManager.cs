using Audio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        private Image _introImage;
        private Canvas _introCanvas;

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
            _introImage = GameObject.FindGameObjectWithTag("Intro").GetComponentInChildren<Image>();
            _introCanvas = GameObject.FindGameObjectWithTag("Intro").GetComponent<Canvas>();
            _introCanvas.enabled = false;
        }

        internal void SwitchState(GameState state)
        {
            State = state;

            switch (state)
            {
                case GameState.MainMenu:
                    Lives = 3;
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
                    _introCanvas.enabled = true;
                    _introImage.DOFade(1, 1f);
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
                _introImage = GameObject.FindGameObjectWithTag("Intro").GetComponentInChildren<Image>();
                _introCanvas = GameObject.FindGameObjectWithTag("Intro").GetComponent<Canvas>();
                _introCanvas.enabled = false;
            }
        }

        #endregion


        private void Update()
        {
            if (Input.anyKey && State == GameState.LevelOne)
            {
                SwitchState(GameState.MainMenu);
                SceneManager.LoadSceneAsync(LEVEL_ONE_PARAM); 
                AudioManager.Instance.FadeGameTrack(Track._001_Ambient_01, Fade.In);
                AudioManager.Instance.FadeGameTrack(Track._002_Ambient_02, Fade.In);
            }
        }
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
