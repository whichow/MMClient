// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMusicMixer" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class KMusicMixer : MonoBehaviour
    {
        [Serializable]
        public class QueuedMusic
        {
            public int repeats;
            public AudioClip musicClip;
        }

        public static KMusicMixer Instance;

        private QueuedMusic _lastPlayedMusic;
        [SerializeField]
        private List<QueuedMusic> _musicQueue = new List<QueuedMusic>();

        public void RequestMusic(AudioClip[] musicClips)
        {
            _musicQueue.Clear();
            QueuedMusic queuedMusic = null;
            for (int i = 0; i < musicClips.Length; i++)
            {
                AudioClip audioClip = musicClips[i];
                if (queuedMusic != null && queuedMusic.musicClip == audioClip)
                {
                    queuedMusic.repeats++;
                }
                else
                {
                    queuedMusic = new QueuedMusic
                    {
                        musicClip = audioClip
                    };
                    _musicQueue.Add(queuedMusic);
                }
            }
            this.PlayNext();
        }

        private void PlayNext()
        {
            if (this._musicQueue.Count > 0)
            {
                QueuedMusic queuedMusic = _musicQueue[0];
                KMusicLayer currentMusicLayer = KSoundManager.CurrentMusicLayer;
                if (currentMusicLayer && currentMusicLayer.GetComponent<AudioSource>().clip != queuedMusic.musicClip)
                {
                    KSoundManager.PlayMusic(queuedMusic.musicClip, true);
                }
                _musicQueue.RemoveAt(0);
                _musicQueue.Add(queuedMusic);
                _lastPlayedMusic = queuedMusic;
            }
        }

        private void Awake()
        {
            KMusicMixer.Instance = this;
        }

        private void Update()
        {
            if (this._lastPlayedMusic != null)
            {
                KMusicLayer currentMusicLayer = KSoundManager.CurrentMusicLayer;
                if (currentMusicLayer && currentMusicLayer.GetComponent<AudioSource>().clip == _lastPlayedMusic.musicClip && currentMusicLayer.numberOfPlays > _lastPlayedMusic.repeats)
                {
                    this.PlayNext();
                }
            }
            else
            {
                this.PlayNext();
            }
        }
    }
}
