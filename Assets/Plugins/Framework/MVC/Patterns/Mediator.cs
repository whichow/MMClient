// ***********************************************************************
// Company          : KimCh
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : KimCh
// Last Modified On : KimCh
// ***********************************************************************

#region Using

using System.Collections.Generic;

#endregion

namespace K.Patterns
{
    /// <summary>
    /// 中介者模式
    /// A base <c>IMediator</c> implementation
    /// </summary>
    /// <see cref="Patterns.Core.View"/>
    public class Mediator : Notifier, IMediator
    {
        #region Constants

        /// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        public const string NAME = "Mediator";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new mediator with the default name and no view component
        /// </summary>
        public Mediator()
            : this(NAME, null)
        {
        }

        /// <summary>
        /// Constructs a new mediator with the specified name and no view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        public Mediator(string mediatorName)
            : this(mediatorName, null)
        {
        }

        /// <summary>
        /// Constructs a new mediator with the specified name and view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        /// <param name="viewComponent">The view component to be mediated</param>
		public Mediator(string mediatorName, object viewComponent)
        {
            _mediatorName = (mediatorName != null) ? mediatorName : NAME;
            _viewComponent = viewComponent;
        }

        #endregion

        #region IMediator Members

        /// <summary>
        /// List the <c>INotification</c> names this <c>Mediator</c> is interested in being notified of
        /// </summary>
        /// <returns>The list of <c>INotification</c> names </returns>
        public virtual IList<string> ListNotificationInterests()
        {
            return new List<string>();
        }

        /// <summary>
        /// Handle <c>INotification</c>s
        /// </summary>
        /// <param name="notification">The <c>INotification</c> instance to handle</param>
        public virtual void HandleNotification(INotification notification)
        {
        }

        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        public virtual void OnRegister()
        {
        }

        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        public virtual void OnRemove()
        {
        }

        #endregion

        #region Accessors

        /// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        public virtual string mediatorName
        {
            get { return _mediatorName; }
        }

        /// <summary>
        /// The <code>IMediator</code>'s view component.
        /// </summary>
        public virtual object viewComponent
        {
            get { return _viewComponent; }
            set { _viewComponent = value; }
        }

        #endregion

        #region Members

        /// <summary>
        /// The mediator name
        /// </summary>
        protected string _mediatorName;

        /// <summary>
        /// The view component being mediated
        /// </summary>
        protected object _viewComponent;

        #endregion
    }
}
