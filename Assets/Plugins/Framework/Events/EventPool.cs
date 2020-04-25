// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace K.Events
{
    /// <summary>
    /// 事件池。
    /// </summary>
    /// <typeparam name="T">事件类型。</typeparam>
    internal sealed class EventPool<T> where T : EventParams
    {
        public enum Mode
        {
            /// <summary>
            /// 有且只有一个事件处理函数。
            /// </summary>
            fDefault = 0,
            /// <summary>
            /// 允许不存在事件处理函数。
            /// </summary>
            fRequireReceiver = 1,
            /// <summary>
            /// 允许存在多个事件处理函数。
            /// </summary>
            fAllowMulticast = 2,
        }

        #region Model  

        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class EventNode
        {
            /// <summary>
            /// 
            /// </summary>
            private readonly object _sender;
            /// <summary>
            /// 
            /// </summary>
            private readonly T _eventParams;

            public EventNode(object sender, T eventParams)
            {
                _sender = sender;
                _eventParams = eventParams;
            }

            public object sender
            {
                get
                {
                    return _sender;
                }
            }

            public T eventParams
            {
                get
                {
                    return _eventParams;
                }
            }
        }

        #endregion

        #region Field

        /// <summary>
        /// 
        /// </summary>
        private readonly Mode _mode;
        /// <summary>
        /// 
        /// </summary>
        private readonly Queue<EventNode> _eventNodes = new Queue<EventNode>();
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<int, EventHandler<T>> _eventHandlers = new Dictionary<int, EventHandler<T>>();

        #endregion

        public EventPool()
        {
            _mode = Mode.fAllowMulticast;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public EventPool(Mode mode)
        {
            _mode = mode;
        }

        /// <summary>
        /// 获取事件数量
        /// </summary>
        public int count
        {
            get
            {
                return _eventNodes.Count;
            }
        }

        /// <summary>
        /// 检查订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                LogUtil.LogError("Event handler is null.");
                return false;
            }

            EventHandler<T> eventHandler;
            if (!_eventHandlers.TryGetValue(id, out eventHandler) ||
                eventHandler == null)
            {
                return false;
            }

            var invocations = eventHandler.GetInvocationList();
            foreach (EventHandler<T> invocation in invocations)
            {
                if (invocation == handler)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                LogUtil.LogError("Event handler is null.");
                return;
            }

            EventHandler<T> eventHandler;
            if (!_eventHandlers.TryGetValue(id, out eventHandler) || eventHandler == null)
            {
                _eventHandlers[id] = handler;
            }
            else if ((_mode & Mode.fAllowMulticast) == 0)
            {
                LogUtil.LogErrorFormat("Event '{0}' not allow multiple handler.", id.ToString());
            }
            else
            {
                eventHandler += handler;
                _eventHandlers[id] = eventHandler;
            }
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            EventHandler<T> eventHandler;
            if (_eventHandlers.TryGetValue(id, out eventHandler))
            {
                if (handler == null)
                {
                    _eventHandlers[id] = null;
                }
                else
                {
                    _eventHandlers[id] -= handler;
                }
            }
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        public void Send(object sender, T eventParams)
        {
            var eventNode = new EventNode(sender, eventParams);
            lock (_eventNodes)
            {
                _eventNodes.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        public void SendImmediate(object sender, T eventParams)
        {
            HandleEvent(sender, eventParams);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            while (_eventNodes.Count > 0)
            {
                EventNode node = null;
                lock (_eventNodes)
                {
                    node = _eventNodes.Dequeue();
                }

                HandleEvent(node.sender, node.eventParams);
            }
        }

        /// <summary>
        /// 关闭并清理事件池
        /// </summary>
        public void Close()
        {
            lock (_eventNodes)
            {
                _eventNodes.Clear();
            }

            _eventHandlers.Clear();
        }

        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventParams"></param>
        private void HandleEvent(object sender, T eventParams)
        {
            EventHandler<T> eventHandler = null;
            if (_eventHandlers.TryGetValue(eventParams.eventID, out eventHandler))
            {
                if (eventHandler != null)
                {
                    eventHandler(sender, eventParams);
                    return;
                }
            }

            if ((_mode & Mode.fRequireReceiver) != 0)
            {
                LogUtil.LogErrorFormat("Event '{0}' not allow no handler.", eventParams.eventID);
            }
        }
    }
}

