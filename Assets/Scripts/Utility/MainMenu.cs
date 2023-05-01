using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Audio;
using System;

namespace Utilities
{
    internal class MainMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _menuCanvas;

        private readonly string START_BUTTON_PARAM = "StartButton";
        private readonly string SETTINGS_BUTTON_PARAM = "SettingsButton";
        private readonly string CREDITS_BUTTON_PARAM = "CreditsButton";
        private readonly string HOWTO_BUTTON_PARAM = "HowToPlayButton";
        private readonly string TITLE_PARAM = "GameTitle";
        private readonly float _startButtonXAnimationDistance = 200f;
        private readonly float _settingsButtonXAnimationDistance = 400;
        private readonly float _creditsButtonXAnimationDistance = 600;
        private readonly float _howToPlayButtonXAnimationDistance = -750;
        private readonly float _buttonAnimationDuration = 1f;
        private readonly float _titleYAnimationDistance = 150f;
        private readonly float _titleAnimationDuration = 2f;

        private GameObject _startButton;
        private GameObject _settingsButton;
        private GameObject _creditsButton;
        private GameObject _howToPlayButton;
        private Image _gameTitle;

        #endregion

        #region Functions

        private void Awake()
        {
            _menuCanvas = GetComponent<Canvas>();
            _menuCanvas.enabled = false;
        }

        private void Start()
        {
            _startButton = GameObject.FindGameObjectWithTag(START_BUTTON_PARAM);
            _settingsButton = GameObject.FindGameObjectWithTag(SETTINGS_BUTTON_PARAM);
            _creditsButton = GameObject.FindGameObjectWithTag(CREDITS_BUTTON_PARAM);
            _howToPlayButton = GameObject.FindGameObjectWithTag(HOWTO_BUTTON_PARAM);
            _gameTitle = GameObject.FindGameObjectWithTag(TITLE_PARAM).GetComponent<Image>();

            GameManager.Instance.MainMenuOpened?.AddListener(OnMainMenuOpened);
            GameManager.Instance.SettingsOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.HowToPlayOpened?.AddListener(OnOtherMenuOpened);

            StartCoroutine(AnimateMenuUI());

            _startButton.GetComponent<Button>().onClick.AddListener(StartGame);
            _settingsButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
            _creditsButton.GetComponent<Button>().onClick.AddListener(OpenCredits);
            _howToPlayButton.GetComponent<Button>().onClick.AddListener(OpenHowTo);
        }

        private void OpenHowTo()
        {
            GameManager.Instance.SwitchState(GameState.HowToPlayMenu);
            PlayButtonClick();
        }

        private void OnDisable()
        {
            GameManager.Instance.MainMenuOpened?.RemoveListener(OnMainMenuOpened);
            GameManager.Instance.SettingsOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.HowToPlayOpened?.RemoveListener(OnOtherMenuOpened);
        }

        private IEnumerator AnimateMenuUI()
        {
            AnimateTitle();
            yield return new WaitForSeconds(_titleAnimationDuration);
            AnimateButton(_startButton, _startButtonXAnimationDistance);
            AnimateButton(_howToPlayButton, _howToPlayButtonXAnimationDistance);
            yield return new WaitForSeconds(0.2f);
            AnimateButton(_settingsButton, _settingsButtonXAnimationDistance);
            yield return new WaitForSeconds(0.2f);
            AnimateButton(_creditsButton, _creditsButtonXAnimationDistance);
            yield return new WaitForSeconds(0.2f);
        }

        private void AnimateButton(GameObject button, float buttonXAnimationDistance)
        {
            button.GetComponent<RectTransform>().DOLocalMoveX(buttonXAnimationDistance, _buttonAnimationDuration).SetEase(Ease.OutBounce);
        }

        private void AnimateTitle()
        {
            _gameTitle.rectTransform.DOLocalMoveY(_titleYAnimationDistance, _titleAnimationDuration).SetEase(Ease.Linear);
        }

        private void OnMainMenuOpened()
        {
            _menuCanvas.enabled = true;
        }

        private void OnOtherMenuOpened()
        {
            _menuCanvas.enabled = false;
        }

        public void OpenSettings()
        {
            GameManager.Instance.SwitchState(GameState.SettingsMenu);
            PlayButtonClick();
        }

        public void OpenCredits()
        {
            GameManager.Instance.SwitchState(GameState.CreditsMenu);
            PlayButtonClick();
        }

        public void StartGame()
        {
            GameManager.Instance.SwitchState(GameState.LevelOne);
            PlayButtonClick();
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._001_ButtonClick);
        }

        #endregion
    }
}
