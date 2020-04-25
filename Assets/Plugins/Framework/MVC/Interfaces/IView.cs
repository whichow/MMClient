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
    /// 视图
    /// The interface definition for a MVC View
    /// </summary>
    public interface IView
    {
        #region Observer

        /// <summary>
        /// Register an <c>IObserver</c> to be notified of <c>INotifications</c> with a given name
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotifications</c> to notify this <c>IObserver</c> of</param>
        /// <param name="observer">The <c>IObserver</c> to register</param>
        void RegisterObserver(string notificationName, IObserver observer);

        /// <summary>
        /// Remove a group of observers from the observer list for a given Notification name.
        /// </summary>
        /// <param name="notificationName">which observer list to remove from</param>
        /// <param name="notifyContext">removed the observers with this object as their notifyContext</param>
        void RemoveObserver(string notificationName, object notifyContext);

        /// <summary>
        /// Notify the <c>IObservers</c> for a particular <c>INotification</c>
        /// </summary>
        /// <param name="note">The <c>INotification</c> to notify <c>IObservers</c> of</param>
        /// <remarks>
        ///     <para>All previously attached <c>IObservers</c> for this <c>INotification</c>'s list are notified and are passed a reference to the <c>INotification</c> in the order in which they were registered</para>
        /// </remarks>
		void NotifyObservers(INotification note);

        #endregion

        #region Mediator

        /// <summary>
        /// Register an <c>IMediator</c> instance with the <c>View</c>
        /// </summary>
        /// <param name="mediator">A a reference to the <c>IMediator</c> instance</param>
        /// <remarks>
        ///     <para>Registers the <c>IMediator</c> so that it can be retrieved by name, and further interrogates the <c>IMediator</c> for its <c>INotification</c> interests</para>
        ///     <para>If the <c>IMediator</c> returns any <c>INotification</c> names to be notified about, an <c>Observer</c> is created encapsulating  the <c>IMediator</c> instance's <c>handleNotification</c> method and registering it as an <c>Observer</c> for all <c>INotifications</c> the <c>IMediator</c> is interested in</para>
        /// </remarks>
        void RegisterMediator(IMediator mediator);

        /// <summary>
        /// Retrieve an <c>IMediator</c> from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to retrieve</param>
        /// <returns>The <c>IMediator</c> instance previously registered with the given <c>mediatorName</c></returns>
		IMediator GetMediator(string mediatorName);

        /// <summary>
        /// Remove an <c>IMediator</c> from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
        IMediator RemoveMediator(string mediatorName);

        /// <summary>
        /// Check if a Mediator is registered or not
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to check for</param>
        /// <returns>whether a Mediator is registered with the given <c>mediatorName</c>.</returns>
        bool HasMediator(string mediatorName);

        #endregion
    }
}
