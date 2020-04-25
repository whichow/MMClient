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
    /// 控制器
    /// The interface definition for a MVC Controller
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Register a particular <c>ICommand</c> class as the handler for a particular <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c></param>
        /// <param name="commandType">The <c>Type</c> of the <c>ICommand</c></param>
        void RegisterCommand(string notificationName, System.Type commandType);

        /// <summary>
        /// Execute the <c>ICommand</c> previously registered as the handler for <c>INotification</c>s with the given notification name
        /// </summary>
        /// <param name="notification">The <c>INotification</c> to execute the associated <c>ICommand</c> for</param>
		void ExecuteCommand(INotification notification);

        /// <summary>
        /// Remove a previously registered <c>ICommand</c> to <c>INotification</c> mapping.
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to remove the <c>ICommand</c> mapping for</param>
		void RemoveCommand(string notificationName);

        /// <summary>
        /// Check if a Command is registered for a given Notification.
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to check the <c>ICommand</c> mapping for</param>
        /// <returns>whether a Command is currently registered for the given <c>notificationName</c>.</returns>
        bool HasCommand(string notificationName);
    }
}
