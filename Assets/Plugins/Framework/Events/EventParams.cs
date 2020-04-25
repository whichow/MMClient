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
    public abstract class EventParams : EventArgs
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public abstract int eventID
        {
            get;
        }

        /// <summary>
        /// 安全投递
        /// </summary>
        public void Send(object sender)
        {
            KFramework.EventManager.Send(sender, this);
        }

        /// <summary>
        /// 立即投递
        /// </summary>
        public void SendImmediate(object sender)
        {
            KFramework.EventManager.SendImmediate(sender, this);
        }
    }
}
