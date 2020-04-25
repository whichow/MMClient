using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace Game
{
    public delegate void TweenDelegate();
    public delegate void TweenDelegate<T>(T time);
    public abstract class TweenBase : KFx// MonoBehaviour
    {

        public enum TweenStyle
        {
            Default,
            Once,
            Loop,
            PingPong,
        }
        public enum BeginStyle
        {
            Default,
            Play,
            Stop,
        }

        public enum PlayState
        {
            Default,
            Stop,
            Play,
            PlayBack,
            Suspend,

        }

        TweenDelegate onEndTween;
        TweenDelegate onStartTween;
        TweenDelegate<float> onTweening;
        public AnimationCurve timeCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        //public float FromTime = 0;
        //public float _ToTime = 1;

        [Tooltip("动画播放类型。默认值Default，和Once值一样")]
        public TweenStyle m_AnimationStyle;
        [Tooltip("动画是否开始就播放动画。默认值Default，和Play值一样")]
        public BeginStyle m_BeginStyle;
        private BeginStyle _curBeginStyle;


        private PlayState mPlayState;
        private PlayState _curPlayState;

        private float _curveTimeMin;
        private float _curveTimeMax;
        private float _curveTimeDelta;
        private int _timeCurveKeysLengh;

        private bool _isStart = true;

        /// <summary>
        /// 持续时间
        /// </summary>
        [Tooltip("动画持续时间。")]
        public float Duration = 1f;
        /// <summary>
        /// 延时开始时间 
        /// </summary>
        [Tooltip("动画延时时间。")]
        public float StartTime = 0f;

        private float _curStartTime = 0f;
        /// <summary>
        /// 速度增量
        /// </summary>
        private float _speedDeltaTime
        {
            get
            {
                return Time.deltaTime / Duration;
            }
        }

        /// <summary>
        /// 曲线最大值
        /// </summary>
        private float _maxCurveValue;
        private int _TimeCurveKeysLengh
        {
            get
            {
                return timeCurve.keys.Length;
            }
            set
            {
                _timeCurveKeysLengh = value;
            }
        }
        private float _factorDelta;
        /// <summary>
        /// 增量缩放系数(控制时间增量速度：每秒增减的delta系数)
        /// </summary>
        private float _FactorDelta
        {
            get
            {
                if (Duration == 0)
                {
                    Duration = 1;
                }
                //return 1000f;
                _factorDelta = Mathf.Abs(1f / Duration) * Mathf.Sign(_factorDelta);
                return _factorDelta;
            }
        }
        float timeValue
        {
            get; set;
        }
        //public float Speed = 1;

        private float curValue
        {
            get
            {

                return Mathf.Lerp(0, _curveTimeMax, Time.time);
            }
        }
        protected abstract void TweenUpData(float time, bool isFinish);
        protected virtual void TweenAwake()
        {
        }





        /// <summary>
        /// 获取曲线值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isFinish"></param>
        public void GetCurveValue(float time, bool isFinish)
        {
            tweenFormZeroTime(time, isFinish);
        }
        /// <summary>
        /// 从曲线0时刻开始取值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isFinish"></param>
        private void tweenFormZeroTime(float time, bool isFinish)
        {
            TweenUpData(timeCurve != null ? timeCurve.Evaluate(0 + _curveTimeMax * time) : time, isFinish);
        }
        /// <summary>
        /// 从第一个关键key开始
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isFinish"></param>
        private void tweenFormFirstKey(float time, bool isFinish)
        {
            TweenUpData(timeCurve != null ? timeCurve.Evaluate(_curveTimeMin + _curveTimeDelta * time) : time, isFinish);
        }
        /// <summary>
        /// 获取曲线最大值
        /// </summary>
        /// <returns></returns>
        IEnumerator GetmaxCurveValue()
        {
            //float startTime = Time.time;
            float timeUsed = 0;
            while (true)
            {
                timeUsed += Time.deltaTime;
                //float delta = Time.time - startTime;
                float time = Mathf.Lerp(0, _curveTimeMax, timeUsed);
                float value = timeCurve.Evaluate(time);
                if (value > _maxCurveValue)
                    _maxCurveValue = value;
                if (timeUsed > _curveTimeMax)
                {
                    Debug.Log(_maxCurveValue);
                    //return;
                    yield break;

                }

                // return;
                yield return 0;
            }


        }

        public void StateToggle(PlayState playState)
        {
            _curPlayState = playState;
        }

        public void onEndTweenSet(TweenDelegate tweemDelegate)
        {
            onEndTween = tweemDelegate;
        }
        public void onStartTweenSet(TweenDelegate tweemDelegate)
        {
            onStartTween = tweemDelegate;
        }
        public void onTweeningSet(TweenDelegate<float> tweemDelegate)
        {
            onTweening = tweemDelegate;
        }



        public virtual void Reset(bool isPlay)
        {
            _curStartTime = 0;
            timeValue = 0;
            GetCurveValue(timeValue, false);                //重置位置
            _isStart = true;
            StateToggle(PlayState.Play);
            if (isPlay)
            {
                _curBeginStyle = BeginStyle.Play;
            }
            else
            {

                _curBeginStyle = BeginStyle.Stop;
            }
            //if (/*!mStarted*/)
            //{
            //    //SetStartToCurrentValue();
            //    //SetEndToCurrentValue();
            //}
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            Reset(false);
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Suspend()
        {
            StateToggle(PlayState.Suspend);
        }
        public void Play()
        {
            this.enabled = true;
            _curBeginStyle = BeginStyle.Play;
            StateToggle(PlayState.Play);
        }
        /// <summary>
        /// 重新播放
        /// </summary>
        public void PlayBack()
        {
            this.enabled = true;
            Reset(true);
        }

        private void onUpdate()
        {
            if (_curBeginStyle == BeginStyle.Stop)      //开始播放停止状态
                return;

            if (_curPlayState == PlayState.Suspend)     // 暂停--- 播放状态
                return;

            //等待时间
            {
                float time = Time.time;
                if (_isStart)
                {

                    _isStart = false;
                    _curStartTime = time + StartTime;
                    if (onStartTween != null)
                        onStartTween();
                }
                if (time < _curStartTime) return;
            }

            //样式实现
            {
                timeValue += /*(duration == 0f) ? 1f : amountPerDelta **/ _FactorDelta * Time.deltaTime;
                if (onTweening != null)
                    onTweening(Mathf.Clamp01(timeValue));

                switch (m_AnimationStyle)
                {
                    case TweenStyle.Default:
                    case TweenStyle.Once:
                        {
                            if (timeValue < 1f)
                            {
                                timeValue = Mathf.Clamp01(timeValue);

                            }
                            else
                            {
                                //Reset(false);
                                timeValue = 1;
                                //GetCurveValue(timeValue, true);
                                this.enabled = false;
                                if (onEndTween != null)
                                    onEndTween();
                            }
                            GetCurveValue(timeValue, true);
                            break;
                        }
                    case TweenStyle.Loop:
                        {
                            if (timeValue > 1f)
                                timeValue -= Mathf.Floor(timeValue);  //获取小数部分
                            break;
                        }
                    case TweenStyle.PingPong:
                        {
                            if (timeValue > 1f)
                            {
                                timeValue = 1f - (timeValue - Mathf.Floor(timeValue)); //反向 ，数值越来越小
                                _factorDelta = -_factorDelta;  //增量减
                            }
                            else if (timeValue < 0f)
                            {
                                timeValue = -timeValue;         //符号取反（正数）
                                timeValue -= Mathf.Floor(timeValue);  //获取小数部分
                                _factorDelta = -_factorDelta;       //增量加
                            }
                            break;
                        }
                    default: break;

                }
            }

            if (m_AnimationStyle == TweenStyle.Loop || m_AnimationStyle == TweenStyle.PingPong)
                GetCurveValue(timeValue, false);
        }

        #region Unity
        void Update()
        {
            onUpdate();
        }
        private void Awake()
        {
            if (timeCurve != null)
            {
                _TimeCurveKeysLengh = timeCurve.keys.Length;
                if (_TimeCurveKeysLengh > 0)
                {
                    _curveTimeMax = timeCurve.keys[_TimeCurveKeysLengh - 1].time;
                    _curveTimeMin = timeCurve.keys[0].time;
                    _curveTimeDelta = _curveTimeMax - _curveTimeMin;
                }
            }

            _curBeginStyle = m_BeginStyle;
            _curPlayState = PlayState.Play;


            //获取曲线最大值
            //StartCoroutine(GetmaxCurveValue());

            TweenAwake();
        }

        void Start()
        {
            //GetCurveValue(0, false);
            //Mathf.Floor

        }


        #endregion
    }
}
