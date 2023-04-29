using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Audio;

namespace Utilities
{
    internal class MainMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _menuCanvas;

        private readonly string START_BUTTON_PARAM = "StartButton";
        private readonly string SETTINGS_BUTTON_PARAM = "SettingsButton";
        private readonly string CREDITS_BUTTON_PARAM = "CreditsButton";
        private readonly string TITLE_PARAM = "GameTitle";
        private readonly float _buttonXAnimationDistance = 200f;
        private readonly float _buttonAnimationDuration = 1f;
        private readonly float _titleYAnimationDistance = 500f;
        private readonly float _titleAnimationDuration = 2f;

        private Button _startButton;
        private Button _settingsButton;
        private Button _creditsButton;
        private TextMeshProUGUI _gameTitle;

        #endregion

        #region Functions

        private void Awake()
        {
            _menuCanvas = GetComponent<Canvas>();
            _menuCanvas.enabled = false;
        }

        private void Start()
        {
            _startButton = GameObject.FindGameObjectWithTag(START_BUTTON_PARAM).GetComponent<Button>();
            _settingsButton = GameObject.FindGameObjectWithTag(SETTINGS_BUTTON_PARAM).GetComponent<Button>();
            _creditsButton = GameObject.FindGameObjectWithTag(CREDITS_BUTTON_PARAM).GetComponent<Button>();
            _gameTitle = GameObject.FindGameObjectWithTag(TITLE_PARAM).GetComponent<TextMeshProUGUI>();

            GameManager.Instance.MainMenuOpened?.AddListener(OnMainMenuOpened);
            GameManager.Instance.SettingsOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.AddListener(OnOtherMenuOpened);

            StartCoroutine(AnimateMenuUI());

            _startButton.onClick.AddListener(StartGame);
            _settingsButton.onClick.AddListener(OpenSettings);
            _creditsButton.onClick.AddListener(OpenCredits);
        }

        private void OnDisable()
        {
            GameManager.Instance.MainMenuOpened?.RemoveListener(OnMainMenuOpened);
            GameManager.Instance.SettingsOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.RemoveListener(OnOtherMenuOpened);
        }

        private IEnumerator AnimateMenuUI()
        {
            AnimateTitle();
            yield return new WaitForSeconds(_titleAnimationDuration);
            AnimateButton(_startButton);
            yield return new WaitForSeconds(0.2f);
            AnimateButton(_settingsButton);
            yield return new WaitForSeconds(0.2f);
            AnimateButton(_creditsButton);
            yield return new WaitForSeconds(0.2f);
        }

        private void AnimateButton(Button button)
        {
            button.transform.DOMoveX(_buttonXAnimationDistance, _buttonAnimationDuration).SetEase(Ease.OutBounce);
        }

        private void AnimateTitle()
        {
            _gameTitle.transform.DOMoveY(_titleYAnimationDistance, _titleAnimationDuration).SetEase(Ease.Linear);
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
