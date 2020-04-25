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
    /// 通知
    /// A Base <c>INotifier</c> implementation
    /// </summary>
    public class Notifier : INotifier
    {
        #region INotifier Members

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notiification to send</param>
        public virtual void SendNotification(string notificationName)
        {
            // The Facade SendNotification is thread safe, therefore this method is thread safe.
            _facade.SendNotification(notificationName);
        }

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
		public virtual void SendNotification(string notificationName, object body)
        {
            // The Facade SendNotification is thread safe, therefore this method is thread safe.
            _facade.SendNotification(notificationName, body);
        }

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
        /// <param name="type">The type of the notification</param>
		public virtual void SendNotification(string notificationName, object body, string type)
        {
            // The Facade SendNotification is thread safe, therefore this method is thread safe.
            _facade.SendNotification(notificationName, body, type);
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Local reference to the Facade Singleton
        /// </summary>
        protected IFacade facade
        {
            get { return _facade; }
        }

        #endregion

        #region Members

        /// <summary>
        /// Local reference to the Facade Singleton
        /// </summary>
        private IFacade _facade = Facade.Instance;

        #endregion
    }
}
