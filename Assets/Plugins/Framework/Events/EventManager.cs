// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System;
using UnityEngine;

namespace K.Events
{
    internal sealed class EventManager : MonoBehaviour, IEventManager
    {
        #region Field

        private readonly EventPool<EventParams> _eventPool = new EventPool<EventParams>();

        #endregion

        #region Proerpty

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int count
        {
            get { return _eventPool.count; }
        }

        #endregion

        #region Method 

        /// <summary>
        /// 检查订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Check(int id, EventHandler<EventParams> handler)
        {
            return _eventPool.Check(id, handler);
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Subscribe(int id, EventHandler<EventParams> handler)
        {
            _eventPool.Subscribe(id, handler);
        }

        /// <summary>
        /// 取消订阅事件。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Unsubscribe(int id, EventHandler<EventParams> handler)
        {
            _eventPool.Unsubscribe(id, handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        public void Send(object sender, EventParams eventParams)
        {
            _eventPool.Send(sender, eventParams);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        public void SendImmediate(object sender, EventParams eventParams)
        {
            _eventPool.SendImmediate(sender, eventParams);
        }

        #endregion

        #region Unity

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            _eventPool.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            _eventPool.Close();
        }

        #endregion
    }
}
