// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System;

namespace K.Events
{
    /// <summary>
    /// 事件管理接口。
    /// </summary>
    public interface IEventManager
    {
        /// <summary>
        /// 获取事件数量。
        /// </summary>
        int count
        {
            get;
        }

        /// <summary>
        /// 检查订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        bool Check(int id, EventHandler<EventParams> handler);

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        void Subscribe(int id, EventHandler<EventParams> handler);

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        void Unsubscribe(int id, EventHandler<EventParams> handler);

        /// <summary>
        /// 事件会在抛出后的下一帧分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        void Send(object sender, EventParams eventParams);

        /// <summary>
        /// 事件会立刻分发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        void SendImmediate(object sender, EventParams eventParams);
    }
}
