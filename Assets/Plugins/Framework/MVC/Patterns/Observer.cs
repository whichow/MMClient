// ***********************************************************************
// Company          : KimCh
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : KimCh
// Last Modified On : KimCh
// ***********************************************************************

#region Using

using System;

#endregion

namespace K.Patterns
{
    /// <summary>
    /// 观察者模式
    /// A base <c>IObserver</c> implementation
    /// </summary>
    public class Observer : IObserver
    {
        #region Constructors

        /// <summary>
        /// Constructs a new observer with the specified notification action and context
        /// </summary>
        /// <param name="notifyTarget">The notification context of the interested object</param>
        /// <param name="notifyAction">The notification action of the interested object</param>
        public Observer(object notifyTarget, Action<INotification> notifyAction)
        {
            _notifyEvent = new Events.SimpleEvent<INotification>();
            _notifyEvent.Add(notifyAction);
            _notifyTarget = notifyTarget;
        }

        #endregion

        #region IObserver Members

        /// <summary>
        /// Notify the interested object
        /// </summary>
        /// <remarks>This method is thread safe</remarks>
        /// <param name="notification">The <c>INotification</c> to pass to the interested object's notification method</param>
        public virtual void NotifyObserver(INotification notification)
        {
            _notifyEvent.Invoke(notification);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(Action<INotification> listener)
        {
            _notifyEvent.Add(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveListener(Action<INotification> listener)
        {
            _notifyEvent.Remove(listener);
        }


        /// <summary>
        /// Compare an object to the notification target
        /// </summary>
        /// <remarks>This method is thread safe</remarks>
        /// <param name="obj">The object to compare</param>
        /// <returns>Indicating if the object and the notification context are the same</returns>
        public virtual bool CompareNotifyTarget(object obj)
        {
            lock (_syncRoot)
            {
                // Compare on the current state
                return notifyTarget == obj;
            }
        }

        #endregion

        #region Accessors 

        /// <summary>
        /// The notification target (this) of the interested object
        /// </summary>
        public virtual object notifyTarget
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return _notifyTarget;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                _notifyTarget = value;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// Holds the notify context.
        /// </summary>
        private object _notifyTarget;

        /// <summary>
        /// Holds the notify method
        /// </summary>
        private Events.SimpleEvent<INotification> _notifyEvent;

        /// <summary>
        /// Used for locking
        /// </summary>
        protected readonly object _syncRoot = new object();

        #endregion
    }
}
