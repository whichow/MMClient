// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;

namespace K.Fsm
{
    /// <summary>
    /// 状态机接口.
    /// </summary>
    public interface IFsm<T> where T : class
    {
        /// <summary>
        /// 获取状态机名称.
        /// </summary>
        string name
        {
            get;
        }

        /// <summary>
        /// 获取状态机拥有者.
        /// </summary>
        T owner
        {
            get;
        }

        /// <summary>
        /// 获取状态机中状态的数量.
        /// </summary>
        int stateCount
        {
            get;
        }

        /// <summary>
        /// 获取状态机是否正在运行.
        /// </summary>
        bool running
        {
            get;
        }

        /// <summary>
        /// 获取状态机是否被销毁.
        /// </summary>
        bool destroyed
        {
            get;
        }

        /// <summary>
        /// 获取当前状态机状态.
        /// </summary>
        FsmState<T> currState
        {
            get;
        }

        /// <summary>
        /// 获取当前状态机状态持续时间.
        /// </summary>
        float currStateTime
        {
            get;
        }

        /// <summary>
        /// 开始状态机.
        /// </summary>
        void Start<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 开始状态机.
        /// </summary>
        /// <param name="stateType"></param>
        void Start(Type stateType);

        /// <summary>
        /// 是否存在状态机状态.
        /// </summary>
        /// <returns></returns>
        bool HasState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 是否存在状态机状态.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        bool HasState(Type stateType);

        /// <summary>
        /// 获取状态机状态.
        /// </summary>
        /// <returns></returns>
        TState GetState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 获取状态机状态.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        FsmState<T> GetState(Type stateType);

        /// <summary>
        /// 切换当前状态机状态.
        /// </summary>
        void ChangeState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// 抛出状态机事件.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventID"></param>
        /// <param name="userData"></param>
        void SendEvent(object sender, int eventID, object userData = null);

        /// <summary>
        /// 是否存在状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasData(string name);

        /// <summary>
        /// 获取状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetData(string name);

        /// <summary>
        /// 设置状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        void SetData(string name, object data);

        /// <summary>
        /// 移除状态机数据.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveData(string name);
    }
}
