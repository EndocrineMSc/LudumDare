using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    internal class HowToPlayCanvas : MonoBehaviour
    {
        private Canvas _menuCanvas;
        private Button _backButton;

        private void Awake()
        {
            _menuCanvas = GetComponent<Canvas>();
            _menuCanvas.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            _backButton = GetComponentInChildren<Button>();
            GameManager.Instance.MainMenuOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.SettingsOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.HowToPlayOpened?.AddListener(OnHowToOpened);
            _backButton.onClick.AddListener(OpenMainMenu);

        }

        private void OnDisable()
        {
            GameManager.Instance.MainMenuOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.SettingsOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.HowToPlayOpened?.RemoveListener(OnHowToOpened);
        }

        private void OnHowToOpened()
        {
            _menuCanvas.enabled = true;
        }

        private void OnOtherMenuOpened()
        {
            _menuCanvas.enabled = false;
        }

        private void OpenMainMenu()
        {
            GameManager.Instance.SwitchState(GameState.MainMenu);
            PlayButtonClick();
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._001_ButtonClick);
        }
    }
}
