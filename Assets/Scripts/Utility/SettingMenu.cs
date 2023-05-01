using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    internal class SettingMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _settingsCanvas;
        private Button _backButton;

        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _sfxVolume;

        #endregion

        #region Functions

        private void Awake()
        {
            _settingsCanvas = GetComponent<Canvas>();
            _settingsCanvas.enabled = false;
        }

        private void Start()
        {
            _backButton = GetComponentInChildren<Button>();

            GameManager.Instance.MainMenuOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.SettingsOpened?.AddListener(OnSettingsOpened);
            GameManager.Instance.CreditsOpened?.AddListener(OnOtherMenuOpened);
            GameManager.Instance.HowToPlayOpened?.AddListener(OnOtherMenuOpened);

            _backButton.onClick.AddListener(OpenMainMenu);

            AudioOptionManager _audioOptions = AudioOptionManager.Instance;

            _masterVolume.onValueChanged.AddListener(_audioOptions.SetMasterVolume);
            _musicVolume.onValueChanged.AddListener(_audioOptions.SetMusicVolume);
            _sfxVolume.onValueChanged.AddListener(_audioOptions.SetEffectsVolume);
        }

        private void OnDisable()
        {
            GameManager.Instance.HowToPlayOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.MainMenuOpened?.RemoveListener(OnOtherMenuOpened);
            GameManager.Instance.SettingsOpened?.RemoveListener(OnSettingsOpened);
            GameManager.Instance.CreditsOpened?.RemoveListener(OnOtherMenuOpened);
        }

        private void OnSettingsOpened()
        {
            _settingsCanvas.enabled = true;
        }
        
        private void OnOtherMenuOpened()
        {
            _settingsCanvas.enabled = false;
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
