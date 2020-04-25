// ***********************************************************************
// Company          : KimCh
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : KimCh
// Last Modified On : KimCh
// ***********************************************************************

namespace K.Patterns
{
    /// <summary>
    /// 观察者模式
    /// The interface definition for a MVC Observer
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// The notification context (this) of the interested object
        /// </summary>
        object notifyTarget { set; }

        /// <summary>
        /// Notify the interested object
        /// </summary>
        /// <param name="notification">The <c>INotification</c> to pass to the interested object's notification method</param>
        void NotifyObserver(INotification notification);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        void AddListener(System.Action<INotification> listener);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        void RemoveListener(System.Action<INotification> listener);

        /// <summary>
        /// Compare the given object to the notificaiton context object
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Indicates if the notification context and the object are the same.</returns>
        bool CompareNotifyTarget(object obj);
    }
}
