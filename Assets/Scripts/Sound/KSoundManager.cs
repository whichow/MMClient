// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KSoundManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Class Sound Manager Aduio or Music
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class KSoundManager : MonoBehaviour
    {
        public float fadeInDuration = 0.6f;
        public float fadeOutDuration = 0.6f;
        public int syncQueueSamples = 800;
        public int syncDelaySamples = 1600;
        public KMusicLayer musicLayerPrefab;

        private bool _audioEnable = true;
        private bool _musicEnable = true;
        private float _soundVolume = 1f;

        private bool _syncMusicLayer;
        private AudioClip _lastMusicClip;
        private AudioSource _audioSource;

        private KMusicLayer _currMusicLayer;
        private KMusicLayer _nextMusicLayer;
        private KMusicLayer _prevMusicLayer;
        private KMusicLayer _shortMusicLayer;

        private const string kAudioEnable = "AudioEnable";
        private const string kMusicEnable = "MusicEnable";
        private const string kSoundVolume = "SoundVolume";

        /// <summary>
        /// Gets or sets a value indicating whether [audio enable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [audio enable]; otherwise, <c>false</c>.
        /// </value>
        private bool audioEnable
        {
            get { return _audioEnable; }
            set
            {
                if (_audioEnable != value)
                {
                    if (KSoundManager.AudioEnableChangedEvent != null)
                    {
                        KSoundManager.AudioEnableChangedEvent(value);
                    }
                    _audioEnable = value;
                    PlayerPrefs.SetInt(kAudioEnable, value ? 1 : 0);
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [music enable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [music enable]; otherwise, <c>false</c>.
        /// </value>
        private bool musicEnable
        {
            get { return _musicEnable; }
            set
            {
                if (_musicEnable != value)
                {
                    if (KSoundManager.MusicEnableChangedEvent != null)
                    {
                        KSoundManager.MusicEnableChangedEvent(value);
                    }
                    _musicEnable = value;
                    PlayerPrefs.SetInt(kMusicEnable, value ? 1 : 0);
                }
            }
        }
        /// <summary>
        /// Gets or sets the music volume scale.
        /// </summary>
        /// <value>
        /// The music volume scale.
        /// </value>
        private float soundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                PlayerPrefs.SetFloat(kSoundVolume, value);

                _currMusicLayer.volume = _soundVolume;
                _nextMusicLayer.volume = _soundVolume;
                _prevMusicLayer.volume = _soundVolume;
                _shortMusicLayer.volume = _soundVolume;
            }
        }

        public AudioSource audioSource
        {
            get { return _audioSource; }
        }

        /// <summary>
        /// Does the play audio.
        /// </summary>
        /// <param name="clip">The clip.</param>
        private void DoPlayAudio(AudioClip clip)
        {
            if (_audioEnable && clip)
            {
                _audioSource.loop = false;
                _audioSource.PlayOneShot(clip);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        private void DoStopAudio(AudioClip clip)
        {
            if (_audioEnable && clip)
            {
                _audioSource.Stop();
            }
        }
        /// <summary>
        /// Does the play audio random.
        /// </summary>
        /// <param name="clips">The clips.</param>
        private void DoPlayAudioRandom(AudioClip[] clips)
        {
            if (_audioEnable && clips.Length > 0)
            {
                AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
                _audioSource.loop = false;
                _audioSource.PlayOneShot(clip);
            }
        }
        /// <summary>
        /// Does the stop music.
        /// </summary>
        private void DoStopMusic()
        {
            if (_nextMusicLayer)
            {
                _nextMusicLayer.Stop(this.fadeOutDuration);
            }
            if (_currMusicLayer)
            {
                _currMusicLayer.Stop(this.fadeOutDuration);
            }
            _lastMusicClip = null;
        }
        /// <summary>
        /// Does the play music.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="syncTime">if set to <c>true</c> [sync time].</param>
        private void DoPlayMusic(AudioClip clip, bool syncTime)
        {
            if (!clip)
            {
                return;
            }

            if (!_musicEnable)
            {
                _lastMusicClip = clip;
                return;
            }

            if (_currMusicLayer.IsPlaying() && clip == _currMusicLayer.clip)
            {
                return;
            }

            _nextMusicLayer.Preload(clip, true);
            _syncMusicLayer = (syncTime && _currMusicLayer.IsPlaying());
        }
        /// <summary>
        /// Does the play interlude.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="loop">if set to <c>true</c> [loop].</param>
        private void DoPlayInterlude(AudioClip clip, bool loop)
        {
            if (_musicEnable)
            {
                _shortMusicLayer.Preload(clip, loop);
            }
        }
        /// <summary>
        /// Does the stop interlude.
        /// </summary>
        private void DoStopInterlude()
        {
            if (_shortMusicLayer)
            {
                _shortMusicLayer.Stop(this.fadeOutDuration);
            }
        }
        /// <summary>
        /// Swaps the music layers.
        /// </summary>
        private void SwapMusicLayers()
        {
            KMusicLayer tmpLayer = _currMusicLayer;
            _currMusicLayer = _nextMusicLayer;
            _nextMusicLayer = _prevMusicLayer;
            _prevMusicLayer = tmpLayer;
            _prevMusicLayer.Stop(0f);
#if DEBUG_MY
            _currMusicLayer.name = "currMusicLayer";
            _nextMusicLayer.name = "nextMusicLayer";
            _prevMusicLayer.name = "prevMusicLayer";
#endif
        }

        #region MonoBehaviour

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            KSoundManager.Instance = this;
        }

        private void Start()
        {
            _audioSource = this.GetComponent<AudioSource>();
            _currMusicLayer = TransformUtils.Instantiate(this.musicLayerPrefab, this.transform);
            _currMusicLayer.gameObject.SetActive(true);

            _nextMusicLayer = TransformUtils.Instantiate(this.musicLayerPrefab, this.transform);
            _nextMusicLayer.gameObject.SetActive(true);

            _prevMusicLayer = TransformUtils.Instantiate(this.musicLayerPrefab, this.transform);
            _prevMusicLayer.gameObject.SetActive(true);

            _shortMusicLayer = TransformUtils.Instantiate(this.musicLayerPrefab, this.transform);
            _shortMusicLayer.gameObject.SetActive(true);

#if DEBUG_MY
            _currMusicLayer.name = "currMusicLayer";
            _nextMusicLayer.name = "nextMusicLayer";
            _prevMusicLayer.name = "prevMusicLayer";
#endif

            this.audioEnable = (PlayerPrefs.GetInt(kAudioEnable, 1) == 1);
            this.musicEnable = (PlayerPrefs.GetInt(kMusicEnable, 1) == 1);
            this.soundVolume = PlayerPrefs.GetFloat(kSoundVolume, 1f);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private void Update()
        {
            if (_musicEnable)
            {
                if (_shortMusicLayer.IsWaitingToPlay())
                {
                    _currMusicLayer.Pause(0.1f);
                    //_nextMusicLayer.Mute();
                    //_prevMusicLayer.Mute();
                    _shortMusicLayer.Play(0f);
                    return;
                }

                if (_shortMusicLayer.IsPlaying())
                {
                    return;
                }

                if (_currMusicLayer.IsWaitingToPlay())
                {
                    _currMusicLayer.Play(this.fadeInDuration);
                    return;
                }

                if (_nextMusicLayer.IsWaitingToPlay())
                {
                    int timeInSamples = 0;
                    if (_syncMusicLayer)
                    {
                        if (_currMusicLayer.numberOfPlays == 0)
                        {
                            return;
                        }
                        if (_currMusicLayer.clip)
                        {
                            timeInSamples = _currMusicLayer.timeSamples;
                            int tmp = _currMusicLayer.clip.samples - timeInSamples;
                            if (tmp > this.syncQueueSamples)
                            {
                                return;
                            }
                            int samples = _nextMusicLayer.clip.samples;
                            while (timeInSamples > samples)
                            {
                                timeInSamples -= samples;
                            }
                        }
                        timeInSamples += this.syncDelaySamples;
                    }
                    if (_currMusicLayer.IsPlaying())
                    {
                        _currMusicLayer.Stop(this.fadeOutDuration);
                    }
                    _nextMusicLayer.Play(this.fadeInDuration, timeInSamples);
                    this.SwapMusicLayers();
                    return;
                }

                if (_lastMusicClip)
                {
                    _nextMusicLayer.Preload(_lastMusicClip, true);
                    _lastMusicClip = null;
                    return;
                }
            }
            else
            {
                if (_currMusicLayer.IsPlaying())
                {
                    _lastMusicClip = _currMusicLayer.clip;
                    _currMusicLayer.Stop(this.fadeOutDuration);
                }

                if (_shortMusicLayer.IsPlaying())
                {
                    _shortMusicLayer.Stop(this.fadeOutDuration);
                }
            }
        }

        private void LateUpdate()
        {
            if (Camera.main)
            {
                this.transform.position = Camera.main.transform.position;
            }
        }

        //private void OnLevelWasLoaded(int level)
        //{

        //}

        #endregion

        #region Static

        static public KSoundManager Instance;
        static public event Action<bool> AudioEnableChangedEvent;
        static public event Action<bool> MusicEnableChangedEvent;

        static public bool AudioEnable
        {
            get { return KSoundManager.Instance ? KSoundManager.Instance.audioEnable : true; }
            set
            {
                if (KSoundManager.Instance)
                {
                    KSoundManager.Instance.audioEnable = value;
                }
            }
        }

        static public bool MusicEnable
        {
            get { return KSoundManager.Instance ? KSoundManager.Instance.musicEnable : true; }
            set
            {
                if (KSoundManager.Instance)
                {
                    KSoundManager.Instance.musicEnable = value;
                }
            }
        }

        static public float SoundVolume
        {
            get { return KSoundManager.Instance ? KSoundManager.Instance.soundVolume : 1f; }
            set
            {
                if (KSoundManager.Instance)
                {
                    KSoundManager.Instance.soundVolume = value;
                }
            }
        }

        static public KMusicLayer CurrentMusicLayer
        {
            get { return KSoundManager.Instance ? KSoundManager.Instance._currMusicLayer : null; }
        }

        static public void PlayAudio(AudioClip clip)
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoPlayAudio(clip);
            }
        }

        static public void StopAudio(AudioClip clip)
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoStopAudio(clip);
            }
        }
        static public void PlayAudioRandom(AudioClip[] clips)
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoPlayAudioRandom(clips);
            }
        }

        static public void PlayMusic(AudioClip clip, bool syncTime)
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoPlayMusic(clip, syncTime);
            }
        }

        static public void PlayMusic(string path, bool syncTime)
        {
            AudioClip ac = Resources.Load(path) as AudioClip;
            PlayMusic(ac, syncTime);
        }

        static public void StopMusic()
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoStopMusic();
            }
        }

        static public void PlayInterlude(AudioClip clip, bool loop)
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoPlayInterlude(clip, loop);
            }
        }

        static public void StopInterlude()
        {
            if (KSoundManager.Instance)
            {
                KSoundManager.Instance.DoStopInterlude();
            }
        }

        public static void PlayAudio(string sound)
        {
            AudioClip ac = Resources.Load(sound) as AudioClip;
            PlayAudio(ac);
        }

        #endregion
    }
}
