// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KFx" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using System.Collections;

namespace Game
{
    public class KFx : MonoBehaviour
    {
        public enum FreeType
        {
            Nothing,
            Despawn,
            Destory,
            Disable
        }

        /// <summary>
        /// The fx level require level
        /// </summary>
        public KQuality.Level fxLevel;
        /// <summary>
        /// The timer destroy duration value <= 0 don't destory , value > 0 use time destory
        /// </summary>
        [Tooltip("<= 0 don't destory , value > 0 use time destory")]
        public float fxDuration = 1f;
        /// <summary>
        /// The fx loop duration  <= 0 don't loop , value > 0 manual loop
        /// </summary>
        [Tooltip("<= 0 don't loop , value > 0 manual loop")]
        public float fxLoopDuration = -1f;
        /// <summary>
        /// The fx loop duration range
        /// </summary>
        public float fxLoopDurationFilter = 0f;
        /// <summary>
        /// The fx play delay start play delay
        /// </summary>
        public float fxFirstDelay = 0.00001f;
        /// <summary>
        /// The free type
        /// </summary>
        public FreeType freeType = FreeType.Despawn;
        /// <summary>
        /// The particle fx prefab
        /// </summary>
        public GameObject fxPrefab;
        /// <summary>
        /// The sound fx clips
        /// </summary>
        public AudioClip[] soundFxClips;

        public bool isStartFirst = true ;
        /// <summary>
        /// The _sound fx index
        /// </summary>
        private int _soundFxIndex = -1;
        /// <summary>
        /// The _destory timer
        /// </summary>
        private float _destoryTimer;
        /// <summary>
        /// The _FX loop timer
        /// </summary>
        private float _fxLoopTimer;
        /// <summary>
        /// The _audio source
        /// </summary>
        private AudioSource _audioSource;
        /// <summary>
        /// The _particle fx
        /// </summary>
        private ParticleSystem _particleFx;

        public int soundFxIndex
        {
            set
            {
                _soundFxIndex = Mathf.Clamp(value, -1, this.soundFxClips.Length);
            }
        }
        /// <summary>
        /// Gets the sound fx clip.
        /// </summary>
        /// <value>
        /// The sound fx clip.
        /// </value>
        private AudioClip soundFxClip
        {
            get
            {
                if (_soundFxIndex < 0)
                {
                    int length = this.soundFxClips.Length;
                    return this.soundFxClips[length > 1 ? Random.Range(0, length) : 0];
                }
                else
                {
                    return this.soundFxClips[_soundFxIndex];
                }
            }
        }
        /// <summary>
        /// Despawns this instance.
        /// </summary>
        private void Despawn()
        {
            if (this.freeType == FreeType.Despawn)
            {
                KPool.Despawn(this.gameObject);
            }
            else if (this.freeType == FreeType.Disable)
            {
                this.gameObject.SetActive(false);
            }
            else if (this.freeType == FreeType.Destory)
            {
                Destroy(this.gameObject);
            }
            else
            {

            }
        }

        public ParticleSystem particle
        {
            get { return _particleFx; }
        }

        /// <summary>
        /// Plays the particle.
        /// </summary>
        private void PlayParticle()
        {
            if (_particleFx)
            {
                _particleFx.Clear();
                _particleFx.Play();
            }
        }
        /// <summary>
        /// Plays the sound.
        /// </summary>
        private void PlaySound()
        {
            if (KSoundManager.AudioEnable && this.soundFxClips.Length > 0)
            {
                if (_audioSource)
                {
                    if (_audioSource.loop)
                    {
                        _audioSource.clip = this.soundFxClip;
                        _audioSource.Play();
                    }
                    else
                    {
                        _audioSource.PlayOneShot(this.soundFxClip);
                    }
                }
                else
                {
                    KSoundManager.PlayAudio(this.soundFxClip);
                }
            }
        }

        #region MonoBehaviour

        private void Start()
        {
            if (this.soundFxClips.Length > 0)
            {
                _audioSource = this.GetComponent<AudioSource>();
            }

            if ((int)this.fxLevel <= KQuality.DeviceLevel)
            {
                //Debug.Log(particleFxPrefab.name);
                if (this.fxPrefab)
                {
                    _particleFx = TransformUtils.Instantiate(this.fxPrefab, this.transform).GetComponent<ParticleSystem>();
                }
                else if(_particleFx)
                {
                    _particleFx.GetComponentInChildren<ParticleSystem>();
                }
                //_particleFx.gameObject.layer = this.gameObject.layer;
            }
        }

        private void OnEnable()
        {
            _destoryTimer = this.fxDuration;
            _fxLoopTimer = this.fxFirstDelay;
            _soundFxIndex = -1;
            isStartFirst = true;
        }

        public void Reset()
        {
            _destoryTimer = this.fxDuration;
            _fxLoopTimer = this.fxFirstDelay;
            _soundFxIndex = -1;
            isStartFirst = true;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!isStartFirst)
            {
                this.Despawn();
                return;
            }

            if (this.fxDuration > 0f)
            {
                _destoryTimer -= Time.deltaTime;
                if (_destoryTimer < 0)
                {
                    this.Despawn();
                    return;
                }
            }

            if (_fxLoopTimer >= 0f)
            {
                _fxLoopTimer -= Time.deltaTime;
                if (_fxLoopTimer < 0f)
                {
                    _fxLoopTimer = this.fxLoopDuration > 0f ? (this.fxLoopDuration + (this.fxLoopDurationFilter > 0 ? Random.Range(0f, this.fxLoopDurationFilter) : 0f)) : -1f;
                    this.PlayParticle();
                    this.PlaySound();
                }
            }
        }

        #endregion
    }
}