// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-31
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KCatBehaviour" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class KCatBehaviour : MonoBehaviour, IPointerClickHandler
    {
        #region Enum

        public enum State
        {
            kNone,
            kIdle,
            kWalk,
            kEat,
            kFun,
            kWin,
            kSkill,
            kSkillPre,
            kSkilling,
            kSkillPost,
            kUpgrade,
        }

        #endregion

        #region Field

        private const string kIdleKey = "idle";
        private const string kWalkKey = "walk";
        private const string kEatKey = "eat";
        private const string kEatingKey = "eating";
        private const string kFunKey = "fun";
        private const string kWinKey = "win";
        private const string kSkillKey = "act_skill";
        private const string kSkillPreKey = "act_skill_qian";
        private const string kSkillingKey = "act_skill_zhong";
        private const string kSkillPostKey = "act_skill_hou";
        private const string kUpgradeKey = "win";

        private SkeletonAnimation _skeletonAnimation;
        private SkeletonGraphic _skeletonGraphic;

        private State _state;
        private State _nextState;

        private System.Action<State> onStart;
        private System.Action<State> onFinish;

        #endregion

        #region Property

        public int catShopId
        {
            get;
            set;
        }

        public bool playing
        {
            get;
            private set;
        }

        public State state
        {
            get { return _state; }
        }

        #endregion

        #region Method  

        public bool HasAnimation(string animName)
        {
            bool b = false;
            var anims = _skeletonAnimation.Skeleton.Data.Animations.Items;
            foreach (var item in anims)
            {
                if (item.Name == animName)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        public void Idle()
        {
            _state = State.kIdle;
            PlayAnimation(kIdleKey, true);
        }

        public void Walk()
        {
            _state = State.kWalk;
            PlayAnimation(kWalkKey, true);
        }

        public void Eat()
        {
            if (_state == State.kEat)
            {
                PlayAnimation(kEatingKey, false);
            }
            else
            {
                _state = State.kEat;
                PlayAnimation(kEatKey, false);
                PlaySound("eat");
            }
            _nextState = State.kIdle;
        }

        public void Fun()
        {
            _state = State.kFun;
            PlayAnimation(kFunKey, false);
            _nextState = State.kIdle;

            PlaySound("fun");
        }

        public void Win(bool loop)
        {
            _state = State.kWin;
            PlayAnimation(kWinKey, loop);
            _nextState = State.kIdle;
            PlaySound("win");
        }

        public void Skill(bool loop, System.Action<State> onFinish)
        {
            _state = State.kSkill;
            this.onFinish = onFinish;
            PlayAnimation(kSkillKey, loop);
            PlaySound("skill");
        }
        public void SkillPre(bool loop, System.Action<State> onFinish)
        {
            _state = State.kSkillPre;
            this.onFinish = onFinish;
            PlayAnimation(kSkillPreKey, loop);
        }

        public void Skilling(bool loop, System.Action<State> onFinish)
        {
            _state = State.kSkilling;
            this.onFinish = onFinish;
            PlayAnimation(kSkillingKey, loop);
        }

        public void SkillPost(bool loop, System.Action<State> onFinish)
        {
            _state = State.kSkillPost;
            this.onFinish = onFinish;
            PlayAnimation(kSkillPostKey, loop);
        }

        public void Upgrade()
        {
            _state = State.kUpgrade;
            PlayAnimation(kWinKey, false);
            _nextState = State.kIdle;
            PlaySound("win");
        }

        public void ChangeState(State state)
        {
            switch (state)
            {
                case State.kNone:
                case State.kIdle:
                    Idle();
                    break;
                case State.kWalk:
                    Walk();
                    break;
                case State.kEat:
                    Eat();
                    break;
                case State.kFun:
                    if (_state != State.kFun)
                    {
                        Fun();
                    }
                    break;
                case State.kWin:
                    Win(true);
                    break;
                case State.kSkillPre:
                    break;
                case State.kSkilling:
                    break;
                case State.kSkillPost:
                    break;
                case State.kUpgrade:
                    if (_state != State.kUpgrade)
                    {
                        Upgrade();
                    }
                    break;
            }
        }

        private void StartDelegate(TrackEntry trackEntry)
        {
            playing = true;
        }

        private void FinishDelegate(TrackEntry trackEntry)
        {
            playing = false;
            OnFinish();
        }

        private void CompleteDelegate(TrackEntry trackEntry)
        {
            playing = false;
            OnComplete();
        }

        private void OnFinish()
        {
            //if (_state == State.kWin)
            //{
            //    Idle();
            //}
        }

        private void OnComplete()
        {
            if (this.onFinish != null)
            {
                var callback = onFinish;
                onFinish = null;
                callback(state);
            }
            if (_state == State.kEat || _state == State.kFun || _state == State.kUpgrade)
            {
                Idle();
            }
        }

        private void PlayAnimation(string animation, bool loop)
        {
            if (_skeletonAnimation != null && _skeletonAnimation.Skeleton.Data.FindAnimation(animation) != null)
            {
                //_skeletonAnimation.AnimationName = null;
                _skeletonAnimation.loop = loop;
                _skeletonAnimation.AnimationName = animation;                
            }
            else if (_skeletonGraphic != null && _skeletonGraphic.SkeletonData.FindAnimation(animation) != null)
            {
                //_skeletonAnimation.AnimationName = null;
                //_skeletonGraphic.startingLoop = loop;
                //_skeletonGraphic.AnimationName = animation;

                _skeletonGraphic.AnimationState.SetAnimation(0, animation, loop);
            }
            else
            {
                OnComplete();
            }
        }

        private void PlaySound(string name)
        {
            //int a = (catShopId / 100) % 10;
            //int b = catShopId % 10;
            int id = catShopId;// a * 1000 + b;
            AudioClip clip;
            if (KAssetManager.Instance.TryGetSoundAsset(string.Format("Audio/sounds_{0}_{1}", id, name), out clip))
            {
                KSoundManager.PlayAudio(clip);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Random.value < 0.8f)
            {
                ChangeState(State.kFun);
            }
            else
            {
                ChangeState(State.kUpgrade);
            }
        }

        #endregion

        #region Unity

        // Use this for initialization  
        private void Start()
        {
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
            //_skeletonAnimation.state.Start += StartDelegate;
            if (_skeletonAnimation != null)
            {
                _skeletonAnimation.AnimationState.End += FinishDelegate;
                _skeletonAnimation.AnimationState.Complete += CompleteDelegate;
            }
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
            if (_skeletonGraphic != null)
            {
                _skeletonGraphic.AnimationState.End += FinishDelegate;
                _skeletonGraphic.AnimationState.Complete += CompleteDelegate;
            }

            ChangeState(_state);
        }

        #endregion
    }
}
