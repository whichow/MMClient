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
    /// 状态基类.
    /// </summary>
    public abstract class FsmState<T> where T : class
    {
        private readonly Dictionary<int, FsmEventHandler<T>> _eventHandlers = new Dictionary<int, FsmEventHandler<T>>();

        /// <summary>
        /// 状态机状态初始化时调用.
        /// </summary>
        /// <param name="fsm"></param>
        protected internal virtual void OnInit(IFsm<T> fsm)
        {

        }

        /// <summary>
        /// 状态机状态进入时调用.
        /// </summary>
        /// <param name="fsm"></param>
        protected internal virtual void OnEnter(IFsm<T> fsm)
        {

        }

        /// <summary>
        /// 状态机状态离开时调用.
        /// </summary>
        /// <param name="fsm"></param>
        /// 
        protected internal virtual void OnExit(IFsm<T> fsm)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fsm"></param>
        protected internal virtual void Update(IFsm<T> fsm, float delta)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fsm"></param>
        protected internal virtual void UpdatePerSecond(IFsm<T> fsm)
        {

        }

        /// <summary>
        /// 状态机状态销毁时调用.
        /// </summary>
        /// <param name="fsm"></param>
        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {
            _eventHandlers.Clear();
        }

        /// <summary>
        /// 订阅状态机事件.
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="eventHandler"></param>
        protected void SubscribeEvent(int eventID, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                LogUtil.LogError("Event handler is null.");
                return;
            }

            if (!_eventHandlers.ContainsKey(eventID))
            {
                _eventHandlers[eventID] = eventHandler;
            }
            else
            {
                _eventHandlers[eventID] += eventHandler;
            }
        }

        /// <summary>
        /// 取消订阅状态机事件.
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="eventHandler"></param>
        protected void UnsubscribeEvent(int eventID, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                LogUtil.LogError("Event handler is null.");
                return;
            }

            if (_eventHandlers.ContainsKey(eventID))
            {
                _eventHandlers[eventID] -= eventHandler;
            }
        }

        /// <summary>
        /// 切换当前状态机状态.
        /// </summary>
        /// <param name="fsm"></param>
        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            var fsmImpl = fsm as Fsm<T>;
            if (fsmImpl == null)
            {
                LogUtil.LogError("Fsm is null.");
                return;
            }

            fsmImpl.ChangeState<TState>();
        }

        /// <summary>
        /// 切换当前状态机状态.
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="stateType"></param>
        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            var fsmImpl = fsm as Fsm<T>;
            if (fsmImpl == null)
            {
                LogUtil.LogError("Fsm is null.");
                return;
            }

            fsmImpl.ChangeState(stateType);
        }

        /// <summary>
        /// 响应状态机事件时调用.
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="sender"></param>
        /// <param name="eventID"></param>
        /// <param name="userData"></param>
        internal void OnEvent(IFsm<T> fsm, object sender, int eventID, object userData)
        {
            FsmEventHandler<T> eventHandlers = null;
            if (_eventHandlers.TryGetValue(eventID, out eventHandlers))
            {
                if (eventHandlers != null)
                {
                    eventHandlers(fsm, sender, userData);
                }
            }
        }
    }
}
