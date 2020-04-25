// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Threading;

namespace K.Network
{
    using Extension;

    public class SendPool<T> where T : class
    {
        #region CONST 

        private const int STATE_IDLE = 1;
        private const int STATE_SENDING = 2;

        #endregion

        #region FIELD 

        /// <summary>
        /// 
        /// </summary>
        private int _state = STATE_IDLE;
        /// <summary>
        /// 
        /// </summary>
        private Queue<T> _sendPool = new Queue<T>();
        /// <summary>
        /// 
        /// </summary>
        private Action<T> _sendAction;

        #endregion

        #region FUNCTION 

        public SendPool(Action<T> action)
        {
            _sendAction = action;
        }

        public void Clear()
        {
            lock (_sendPool)
            {
                _sendPool.Clear();
            }
        }

        public void Reset()
        {
            _state = STATE_IDLE;
        }

        public void Add(T item)
        {
            lock (_sendPool)
            {
                _sendPool.Enqueue(item);
            }
            TrySend();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrySend()
        {
            if (Interlocked.CompareExchange(ref _state, STATE_SENDING, STATE_IDLE) == STATE_IDLE)
            {
                T tmpT = null;
                lock (_sendPool)
                {
                    if (_sendPool.Count > 0)
                    {
                        tmpT = _sendPool.Dequeue();
                    }
                }

                if (tmpT != null)
                {
                    _sendAction.SafeInvoke(tmpT);
                }
                else
                {
                    _state = STATE_IDLE;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TrySendNext()
        {
            _state = STATE_IDLE;
            TrySend();
        }

        #endregion
    }
}
