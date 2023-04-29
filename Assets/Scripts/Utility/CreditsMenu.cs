using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    internal class CreditsMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _creditsCanvas;
        private Button _backButton;

        #endregion

        #region Functions

        private void Awake()
        {
            _creditsCanvas = GetComponent<Canvas>();
            _creditsCanvas.enabled = false;
        }

        private void Start()
        {
            _backButton = GetComponentInChildren<Button>();

            GameManager.Instance.MainMenuOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.CreditsOpened?.AddListener(OnCreditsOpened);
            GameManager.Instance.SettingsOpened?.AddListener(OnOtherMenuOpened);

            _backButton.onClick.AddListener(OpenMainMenu);
        }

        private void OnCreditsOpened()
        {
            _creditsCanvas.enabled = true;
        }

        private void OnOtherMenuOpened()
        {
            _creditsCanvas.enabled = false;
        }

        private void OpenMainMenu()
        {
            GameManager.Instance.MainMenuOpened?.Invoke();
            PlayButtonClick();
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._001_ButtonClick);
        }

        #endregion
    }
}
