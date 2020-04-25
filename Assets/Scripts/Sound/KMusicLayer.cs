// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMusicLayer" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AudioSource))]
    public class KMusicLayer : MonoBehaviour
    {
        private enum State
        {
            None,
            WaitingToPlay,
            FadingOut,
            FadingIn,
            Playing,
            Pause,
            Stop,
            Mute
        }

        private int _lastSamples;
        private int _numberOfPlays;
        private float _volume;
        private float _fadeDuration;
        private float _fadeStartTime;
        private float _fadeStartVolume;
        private State _state;
        private AudioSource _audioSource;

        /// <summary>
        /// Gets the number of plays.
        /// </summary>
        /// <value>
        /// The number of plays.
        /// </value>
        public int numberOfPlays
        {
            get { return _numberOfPlays; }
        }
        /// <summary>
        /// Gets the time samples.
        /// </summary>
        /// <value>
        /// The time samples.
        /// </value>
        public int timeSamples
        {
            get { return _audioSource.timeSamples; }
        }
        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public float volume
        {
            set
            {
                _volume = value;
                _audioSource.volume = value;
            }
        }
        /// <summary>
        /// Gets the clip.
        /// </summary>
        /// <value>
        /// The clip.
        /// </value>
        public AudioClip clip
        {
            get { return _audioSource.clip; }
        }
        /// <summary>
        /// Preloads the specified clip.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="loop">if set to <c>true</c> [loop].</param>
        public void Preload(AudioClip clip, bool loop)
        {
            _lastSamples = 0;
            _numberOfPlays = 0;
            _fadeStartVolume = 0f;
            _state = State.WaitingToPlay;

            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.timeSamples = 0;
            _audioSource.volume = 0f;
            _audioSource.Play();
        }
        /// <summary>
        /// Determines whether [is waiting to play].
        /// </summary>
        /// <returns></returns>
        public bool IsWaitingToPlay()
        {
            return _state == State.WaitingToPlay;
        }
        /// <summary>
        /// Determines whether [is fading in].
        /// </summary>
        /// <returns></returns>
        public bool IsFadingIn()
        {
            return _state == State.FadingIn;
        }
        /// <summary>
        /// Determines whether [is fading out].
        /// </summary>
        /// <returns></returns>
        public bool IsFadingOut()
        {
            return _state == State.FadingOut;
        }
        /// <summary>
        /// Determines whether this instance is playing.
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return _state == State.FadingIn || _state == State.Playing;
        }
        /// <summary>
        /// Determines whether this instance is stopped.
        /// </summary>
        /// <returns></returns>
        public bool IsStopped()
        {
            return _state != State.FadingIn && _state != State.Playing && _state != State.WaitingToPlay;
        }
        /// <summary>
        /// Plays the specified fade duration.
        /// </summary>
        /// <param name="fadeDuration">Duration of the fade.</param>
        public void Play(float fadeDuration)
        {
            if (_audioSource.clip)
            {
                _fadeDuration = fadeDuration;
                _fadeStartTime = Time.realtimeSinceStartup;
                _fadeStartVolume = 0f;
                _state = State.FadingIn;
            }
        }
        /// <summary>
        /// Plays the specified fade duration.
        /// </summary>
        /// <param name="fadeDuration">Duration of the fade.</param>
        /// <param name="timeInSamples">The time in samples.</param>
        public void Play(float fadeDuration, int timeInSamples)
        {
            if (_audioSource.clip)
            {
                int clipSamples = this.clip.samples;
                int modSamples = timeInSamples % clipSamples;
                _audioSource.timeSamples = modSamples;
                _numberOfPlays = (modSamples < (clipSamples >> 1) ? 1 : 0);
                _lastSamples = timeInSamples;
                this.Play(fadeDuration);
            }
        }
        /// <summary>
        /// Stops the specified fade duration.
        /// </summary>
        /// <param name="fadeDuration">Duration of the fade.</param>
        public void Stop(float fadeDuration)
        {
            _fadeDuration = fadeDuration;
            _fadeStartTime = Time.realtimeSinceStartup;
            _fadeStartVolume = _volume;
            _state = State.FadingOut;
        }
        /// <summary>
        /// Pauses the specified fade duration.
        /// </summary>
        /// <param name="fadeDuration">Duration of the fade.</param>
        public void Pause(float fadeDuration)
        {
            if (this.IsPlaying())
            {
                _fadeDuration = fadeDuration;
                _fadeStartTime = Time.realtimeSinceStartup;
                _fadeStartVolume = _volume;
                _state = State.Pause;
            }
        }
        /// <summary>
        /// Mutes this instance.
        /// </summary>
        public void Mute()
        {
            _state = State.Mute;
            _audioSource.volume = 0f;
        }
        /// <summary>
        /// Checks the loop.
        /// </summary>
        private void CheckLoop()
        {
            //更新速度足够快 大于samples / 2 基本上可以做播放一次~
            int timeSamples = _audioSource.timeSamples;
            AudioClip clip = _audioSource.clip;
            if (clip && timeSamples < _lastSamples - (clip.samples >> 1))
            {
                _numberOfPlays++;
            }
            _lastSamples = timeSamples;
        }
        /// <summary>
        /// Updates the fade.
        /// </summary>
        /// <param name="targetVolume">The target volume.</param>
        /// <returns></returns>
        private bool UpdateFade(float targetVolume)
        {
            float timer = Time.realtimeSinceStartup - _fadeStartTime;
            if (timer > _fadeDuration)
            {
                _audioSource.volume = targetVolume;
                return false;
            }
            float t = Mathf.Log10(Mathf.Lerp(1f, 10f, timer / _fadeDuration));
            _audioSource.volume = Mathf.Lerp(_fadeStartVolume, targetVolume, t);
            return true;
        }

        #region MONOBEHAVIOUR

        private void Awake()
        {
            _audioSource = this.GetComponent<AudioSource>();
            _volume = _audioSource.volume;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.FadingIn:
                    if (!this.UpdateFade(_volume))
                    {
                        _state = State.Playing;
                    }
                    this.CheckLoop();
                    break;
                case State.Playing:
                    if (!_audioSource.isPlaying && !_audioSource.loop)
                    {
                        _state = State.None;
                    }
                    this.CheckLoop();
                    break;
                case State.FadingOut:
                    if (!this.UpdateFade(0f))
                    {
                        _state = State.Stop;
                    }
                    break;
                case State.Stop:
                    _audioSource.Stop();
                    _audioSource.clip = null;
                    _state = State.None;
                    break;
                case State.Pause:
                    if (!this.UpdateFade(0f))
                    {
                        _state = State.WaitingToPlay;
                    }
                    break;
            }
        }

        #endregion
    }
}
