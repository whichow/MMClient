//// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMusicTrack" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using UnityEngine;

namespace Game
{
    public class KMusicTrack : MonoBehaviour
    {
        public static KMusicTrack Instance;

        public AudioClip[] layerClips;

        public void PlayFinalLayer()
        {
            KSoundManager.PlayMusic(this.layerClips[this.layerClips.Length - 1], true);
        }

        public void PlayLayer(int layerIndex)
        {
            AudioClip clip = this.layerClips[Mathf.Clamp(layerIndex, 0, this.layerClips.Length - 1)];
            KSoundManager.PlayMusic(clip, true);
        }

        public void PlayLayerForProgress(float progress)
        {
            int num = (int)(progress * (float)(this.layerClips.Length - 1));
            KSoundManager.PlayMusic(this.layerClips[num], true);
        }

        private void Awake()
        {
            KMusicTrack.Instance = this;
        }
    }
}
