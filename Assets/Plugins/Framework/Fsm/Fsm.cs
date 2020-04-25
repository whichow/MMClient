// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace K.Fsm
{
    /// <summary>
    /// 状态机.
    /// </summary>
    internal sealed class Fsm<T> : FsmBase, IFsm<T> where T : class
    {
        #region Static

        public static string GetFullName(string name)
        {
            return typeof(T).FullName + "." + name;
        }

        #endregion

        #region Field

        private readonly T _owner;
        private readonly Dictionary<string, FsmState<T>> _allStates;
        private readonly Dictionary<string, object> _allDatas;

        private FsmState<T> _currentState;
        private float _currentStateTime;
        private bool _destroyed;

        #endregion

        #region Property

        /// <summary>
        /// 
        /// </summary>
        public T owner
        {
            get
            {
                return _owner;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Type ownerType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int stateCount
        {
            get
            {
                return _allStates.Count;
            }
        }

        /// <summary>
        /// 获取状态机是否正在运行.
        /// </summary>
        public override bool running
        {
            get
            {
                return _currentState != null;
            }
        }

        /// <summary>
        /// 获取状态机是否被销毁.
        /// </summary>
        public override bool destroyed
        {
            get
            {
                return _destroyed;
            }
        }

        /// <summary>
        /// 获取当前状态机状态.
        /// </summary>
        public FsmState<T> currState
        {
            get
            {
                return _currentState;
            }
        }

        /// <summary>
        /// 获取当前状态机状态名称.
        /// </summary>
        public override string currentStateName
        {
            get
            {
                return _currentState != null ? _currentState.GetType().FullName : null;
            }
        }

        /// <summary>
        /// 获取当前状态机状态持续时间.
        /// </summary>
        public override float currStateTime
        {
            get
            {
                return _currentStateTime;
            }
        }

        #endregion

        /// <summary>
        /// 初始化状态机的新实例.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        public Fsm(string name, T owner, params FsmState<T>[] states)
            : base(name)
        {
            if (owner == null)
            {
                LogUtil.LogError("Fsm owner is null.");
                return;
            }

            if (states == null || states.Length < 1)
            {
                LogUtil.LogError("Fsm states params is invalid.");
                return;
            }

            _owner = owner;
            _allStates = new Dictionary<string, FsmState<T>>();
            _allDatas = new Dictionary<string, object>();

            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    LogUtil.LogError("Fsm state is null.");
                    continue;
                }

                string stateName = state.GetType().FullName;
                if (_allStates.ContainsKey(stateName))
                {
                    LogUtil.LogErrorFormat("Fsm '{0}' state '{1}' is already exist.", GetFullName(name), stateName);
                    continue;
                }

                _allStates.Add(stateName, state);
                state.OnInit(this);
            }

            _currentStateTime = 0f;
            _currentState = null;
            _destroyed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void Update(float delta)
        {
            if (_currentState == null)
            {
                return;
            }

            _currentStateTime += delta;
            _currentState.Update(this, delta);
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void UpdatePerSecond()
        {
            if (_currentState == null)
            {
                return;
            }

            _currentState.UpdatePerSecond(this);
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void OnDestroy()
        {
            if (_currentState != null)
            {
                _currentState.OnExit(this);
                _currentState = null;
                _currentStateTime = 0f;
            }

            foreach (var state in _allStates.Values)
            {
                state.OnDestroy(this);
            }

            _allStates.Clear();
            _allDatas.Clear();

            _destroyed = true;
        }

        /// <summary>
        /// 开始状态机.
        /// </summary>
        public void Start<TState>() where TState : FsmState<T>
        {
            Start(typeof(TState));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateType"></param>
        public void Start(Type stateType)
        {
            if (running)
            {
                LogUtil.LogError("Fsm is running, can not start again.");
                return;
            }

            var state = GetState(stateType);
            if (state == null)
            {
                LogUtil.LogErrorFormat("Fsm '{0}' can not start state '{1}' which is not exist.", GetFullName(name), stateType.FullName);
                return;
            }

            _currentStateTime = 0f;
            _currentState = state;
            _currentState.OnEnter(this);
        }

        /// <summary>
        /// 是否存在状态机状态.
        /// </summary>
        /// <returns></returns>
        public bool HasState<TState>() where TState : FsmState<T>
        {
            return HasState(typeof(TState));
        }

        /// <summary>
        /// 是否存在状态机状态.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public bool HasState(Type stateType)
        {
            return _allStates.ContainsKey(stateType.FullName);
        }

        /// <summary>
        /// 获取状态机状态.
        /// </summary>
        /// <returns></returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            return (TState)GetState(typeof(TState));
        }

        /// <summary>
        /// 获取状态机状态.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public FsmState<T> GetState(Type stateType)
        {
            FsmState<T> state = null;
            if (_allStates.TryGetValue(stateType.FullName, out state))
            {
                return state;
            }

            return null;
        }

        /// <summary>
        /// 抛出状态机事件.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventID"></param>
        /// <param name="userData"></param>
        public void SendEvent(object sender, int eventID, object userData = null)
        {
            if (_currentState == null)
            {
                LogUtil.LogError("Current state is invalid.");
                return;
            }

            _currentState.OnEvent(this, sender, eventID, userData);
        }

        /// <summary>
        /// 是否存在状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                LogUtil.LogError("Data name is invalid.");
                return false;
            }

            return _allDatas.ContainsKey(name);
        }

        /// <summary>
        /// 获取状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                LogUtil.LogError("Data name is invalid.");
                return null;
            }

            object data = null;
            if (_allDatas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }

        /// <summary>
        /// 设置状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SetData(string name, object data)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            _allDatas[name] = data;
        }

        /// <summary>
        /// 移除状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return _allDatas.Remove(name);
        }

        /// <summary>
        /// 切换当前状态机状态.
        /// </summary>
        public void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        /// 切换当前状态机状态.
        /// </summary>
        /// <param name="stateType"></param>
        internal void ChangeState(Type stateType)
        {
            if (_currentState == null)
            {
                LogUtil.LogError("Current state is null.");
                return;
            }

            var state = GetState(stateType);
            if (state == null)
            {
                LogUtil.LogErrorFormat("Fsm '{0}' can not change state to '{1}' which is not exist.", GetFullName(name), stateType.FullName);
                return;
            }

            _currentState.OnExit(this);
            _currentStateTime = 0f;
            _currentState = state;
            _currentState.OnEnter(this);
        }
    }
}
