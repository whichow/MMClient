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
    /// 装饰模式
    /// The interface definition for a MVC Facade
    /// </summary>
    public interface IFacade : INotifier
    {
        #region Proxy

        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c> by name
        /// </summary>
        /// <param name="proxy">The <c>IProxy</c> to be registered with the <c>Model</c></param>
        void RegisterProxy(IProxy proxy);

        /// <summary>
        /// Retrieve a <c>IProxy</c> from the <c>Model</c> by name
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> instance to be retrieved</param>
        /// <returns>The <c>IProxy</c> previously regisetered by <c>proxyName</c> with the <c>Model</c></returns>
		IProxy GetProxy(string proxyName);

        /// <summary>
        /// Remove an <c>IProxy</c> instance from the <c>Model</c> by name
        /// </summary>
        /// <param name="proxyName">The <c>IProxy</c> to remove from the <c>Model</c></param>
        IProxy RemoveProxy(string proxyName);

        /// <summary>
        /// Check if a Proxy is registered
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> instance to check</param>
        /// <returns>whether a Proxy is currently registered with the given <c>proxyName</c>.</returns>
        bool HasProxy(string proxyName);

        #endregion

        #region Command

        /// <summary>
        /// Register an <c>ICommand</c> with the <c>Controller</c>
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to associate the <c>ICommand</c> with.</param>
        /// <param name="commandType">A reference to the <c>Type</c> of the <c>ICommand</c></param>
        void RegisterCommand(string notificationName, System.Type commandType);

        /// <summary>
        /// Remove a previously registered <c>ICommand</c> to <c>INotification</c> mapping from the Controller.
        /// </summary>
        /// <param name="notificationName">TRemove a previously registered <c>ICommand</c> to <c>INotification</c> mapping from the Controller.</param>
		void RemoveCommand(string notificationName);

        /// <summary>
        /// Check if a Command is registered for a given Notification 
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to check.</param>
        /// <returns>whether a Command is currently registered for the given <c>notificationName</c>.</returns>
        bool HasCommand(string notificationName);

        #endregion

        #region Mediator

        /// <summary>
        /// Register an <c>IMediator</c> instance with the <c>View</c>
        /// </summary>
        /// <param name="mediator">A reference to the <c>IMediator</c> instance</param>
        void RegisterMediator(IMediator mediator);

        /// <summary>
        /// Retrieve an <c>IMediator</c> instance from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to retrieve</param>
        /// <returns>The <c>IMediator</c> previously registered with the given <c>mediatorName</c></returns>
		IMediator GetMediator(string mediatorName);

        /// <summary>
        /// Remove a <c>IMediator</c> instance from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
        IMediator RemoveMediator(string mediatorName);

        /// <summary>
        /// Check if a Mediator is registered or not
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to check</param>
        /// <returns>whether a Mediator is registered with the given <c>mediatorName</c>.</returns>
        bool HasMediator(string mediatorName);

        #endregion

        #region Observer

        /// <summary>
        /// Notify the <c>IObservers</c> for a particular <c>INotification</c>.
        /// <para>All previously attached <c>IObservers</c> for this <c>INotification</c>'s list are notified and are passed a reference to the <c>INotification</c> in the order in which they were registered.</para>
        /// <para>NOTE: Use this method only if you are sending custom Notifications. Otherwise use the sendNotification method which does not require you to create the Notification instance.</para>
        /// </summary>
        /// <param name="note">the <c>INotification</c> to notify <c>IObservers</c> of.</param>
        void NotifyObservers(INotification note);

        #endregion
    }
}
