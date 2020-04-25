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
    /// 通知者
    /// The interface definition for a MVC Notifier
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Send a <c>INotification</c>
        /// </summary>
        /// <remarks>
        ///     <para>Convenience method to prevent having to construct new notification instances in our implementation code</para>
        /// </remarks>
        /// <param name="notificationName">The name of the notification to send</param>
		void SendNotification(string notificationName);

        /// <summary>
        /// Send a <c>INotification</c>
        /// </summary>
        /// <remarks>
        ///     <para>Convenience method to prevent having to construct new notification instances in our implementation code</para>
        /// </remarks>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
		void SendNotification(string notificationName, object body);

        /// <summary>
        /// Send a <c>INotification</c>
        /// </summary>
        /// <remarks>
        ///     <para>Convenience method to prevent having to construct new notification instances in our implementation code</para>
        /// </remarks>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
        /// <param name="type">The type of the notification</param>
		void SendNotification(string notificationName, object body, string type);
    }
}
