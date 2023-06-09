using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    /// <summary>
    /// Intended as a component to the AudioManager.
    /// Provides functions for master, music and sound effect volume sliders.
    /// The AudioManager set the respective AudioGroups of the AudioSources automatically, depending on the folder the clips were in.
    /// </summary>
    internal class AudioOptionManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static AudioOptionManager Instance { get; private set; }

        [SerializeField] private AudioMixer _audioMixer;

        #endregion

        #region Functions

        /// <summary>
        /// Actual functions for volume sliders.
        /// The actual volume is in decibel, which is on a logarithmic scale.
        /// </summary>
        /// <param name="volume">Float set by the respective slider on a linear scale (between -80 and 20)</param>

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


        public void SetMasterVolume(float volume)
        {
            _audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log(volume) *20f : -80f);
        }

        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("Music", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
        }

        public void SetEffectsVolume(float volume)
        {
            _audioMixer.SetFloat("SFX", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
            
            //Play an exemplary SFX to give the play an auditory volume feedback
            AudioManager.Instance.PlaySoundEffectOnce(SFX._001_ButtonClick);
        }

        #endregion
    }
}
