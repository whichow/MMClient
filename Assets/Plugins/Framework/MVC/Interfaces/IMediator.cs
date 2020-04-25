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
    /// 中介者模式
    /// The interface definition for a MVC Mediator.
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Tthe <c>IMediator</c> instance name
        /// </summary>
        string mediatorName { get; }

        /// <summary>
        /// The <c>IMediator</c>'s view component
        /// </summary>
        object viewComponent { get; set; }

        /// <summary>
        /// List <c>INotification interests</c>
        /// </summary>
        /// <returns>An <c>IList</c> of the <c>INotification</c> names this <c>IMediator</c> has an interest in</returns>
        System.Collections.Generic.IList<string> ListNotificationInterests();

        /// <summary>
        /// Handle an <c>INotification</c>
        /// </summary>
        /// <param name="notification">The <c>INotification</c> to be handled</param>
        void HandleNotification(INotification notification);

        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        void OnRegister();

        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        void OnRemove();
    }
}
