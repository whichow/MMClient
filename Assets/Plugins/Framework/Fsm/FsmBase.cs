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
    /// 状态机基类.
    /// </summary>
    public abstract class FsmBase
    {
        private readonly string _name;

        /// <summary>
        /// 初始化状态机基类的新实例.
        /// </summary>
        public FsmBase()
            : this(null)
        {
        }

        /// <summary>
        /// 初始化状态机基类的新实例.
        /// </summary>
        /// <param name="name"></param>
        public FsmBase(string name)
        {
            _name = name ?? "";
        }

        /// <summary>
        /// 获取状态机名称.
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// 获取状态机拥有者类型.
        /// </summary>
        public abstract Type ownerType
        {
            get;
        }

        /// <summary>
        /// 获取状态机中状态的数量.
        /// </summary>
        public abstract int stateCount
        {
            get;
        }

        /// <summary>
        /// 获取状态机是否正在运行.
        /// </summary>
        public abstract bool running
        {
            get;
        }

        /// <summary>
        /// 获取状态机是否被销毁.
        /// </summary>
        public abstract bool destroyed
        {
            get;
        }

        /// <summary>
        /// 获取当前状态机状态名称.
        /// </summary>
        public abstract string currentStateName
        {
            get;
        }

        /// <summary>
        /// 获取当前状态机状态持续时间.
        /// </summary>
        public abstract float currStateTime
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        internal abstract void Update(float delta);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        internal abstract void UpdatePerSecond();

        /// <summary>
        /// 
        /// </summary>
        internal abstract void OnDestroy();
    }
}
