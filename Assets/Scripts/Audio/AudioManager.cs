using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    internal class AudioManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static AudioManager Instance { get; private set; }

        //Auto-built lists on Awake
        private List<AudioSource> _soundEffects;
        private List<AudioSource> _gameTracks;

        //Groups of the MasterMixer Asset
        [SerializeField] private AudioMixerGroup _SFX;
        [SerializeField] private AudioMixerGroup _music;

        private readonly string TRACKS_PARAM = "GameTracks";
        private readonly string SFX_PARAM = "SoundEffects";

        #endregion

        #region Functions

        internal void FadeGameTrack(Track track, Fade fade)
        {
            AudioSource audioSource = _gameTracks[(int)track];   

            if (fade == Fade.In)
            {
                StartCoroutine(StartFade(audioSource, 3f, 1f));
            }
            else if (fade == Fade.Out) 
            {
                StartCoroutine(StartFade(audioSource, 3f, 0f));
            }
        }

        // Start playing a GameTrack attached to the manager if it isn't playing already (for param see above)
        internal void PlayGameTrack(Track track)
        {
            AudioSource audioSource = _gameTracks[(int)track];

            if (!audioSource.isPlaying)
            { 
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index, if it isn't playing already
        internal void PlaySoundEffectOnce(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index, even if it is playing already
        internal void PlaySoundEffectWithoutLimit(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];
            audioSource.Play();
        }

        private void Awake()
        {
            // If there is an instance, and it's not this one, delete this one
            // Instantiation of the singleton, will stay for all scenes
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);

            //Builds the Lists to be used from the Assets/Resources Folder
            _gameTracks = BuildGameTrackList();
            _soundEffects = BuildSoundEffectList();
        }

        private void Start()
        {
            PlayGameTrack(Track._003_Pixie_Makeover);
            PlayGameTrack(Track._002_Ambient_02);
            PlayGameTrack(Track._003_Pixie_Makeover);
            FadeGameTrack(Track._003_Pixie_Makeover, Fade.In);
        }

        private List<AudioSource> BuildGameTrackList()
        {
            AudioClip[] _gameTrackArray = Resources.LoadAll<AudioClip>(TRACKS_PARAM);
            List<AudioSource> _tempList = new();

            foreach (AudioClip _clip in _gameTrackArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.loop = true;
                audioSource.volume = 0;
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = _music;

                _tempList.Add(audioSource);
            }
            return _tempList;
        }

        private List<AudioSource> BuildSoundEffectList()
        {
            AudioClip[] _soundEffectArray = Resources.LoadAll<AudioClip>(SFX_PARAM);
            List<AudioSource> _tempList = new();

            foreach (AudioClip _clip in _soundEffectArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.outputAudioMixerGroup = _SFX;
                audioSource.playOnAwake = false;

                _tempList.Add(audioSource);
            }
            return _tempList;
        }

        private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float startVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }

        #endregion
    }

    internal enum Fade
    {
        In,
        Out,
    }

    internal enum Track
    {
        _001_Ambient_01,
        _002_Ambient_02,
        _003_Pixie_Makeover
    }

    internal enum SFX
    {
        _001_ButtonClick,
        _002_Enemy_Death_02,
        _003_Jump_01,
        _004_Player_Death_02,
        _005_Wurf_02,
        _006_step_left,
    }
}