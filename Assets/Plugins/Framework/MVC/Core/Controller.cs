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
using System.Collections.Generic;

#endregion

namespace K.Patterns
{
    /// <summary>
    /// 控制器
    /// A Singleton <c>IController</c> implementation.
    /// </summary>
    public class Controller : IController
    {
        #region Constructors

        /// <summary>
        /// Constructs and initializes a new controller
        /// </summary>
        protected Controller()
        {
            _commandMap = new Dictionary<string, Type>();
            InitializeController();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// If an <c>ICommand</c> has previously been registered
        /// to handle a the given <c>INotification</c>, then it is executed.
        /// </summary>
        /// <param name="note">An <c>INotification</c></param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void ExecuteCommand(INotification note)
        {
            Type commandType = null;

            lock (_syncRoot)
            {
                if (!_commandMap.TryGetValue(note.name, out commandType))
                {
                    return;
                }
            }

            object commandInstance = Activator.CreateInstance(commandType);

            if (commandInstance is ICommand)
            {
                ((ICommand)commandInstance).Execute(note);
            }
        }

        /// <summary>
        /// Register a particular <c>ICommand</c> class as the handler
        /// for a particular <c>INotification</c>.
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c></param>
        /// <param name="commandType">The <c>Type</c> of the <c>ICommand</c></param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RegisterCommand(string notificationName, Type commandType)
        {
            lock (_syncRoot)
            {
                if (!_commandMap.ContainsKey(notificationName))
                {
                    // This call needs to be monitored carefully. Have to make sure that RegisterObserver
                    // doesn't call back into the controller, or a dead lock could happen.
                    _view.RegisterObserver(notificationName, new Observer(this, this.ExecuteCommand));
                }

                _commandMap[notificationName] = commandType;
            }
        } 
        
        /// <summary>
        /// Check if a Command is registered for a given Notification 
        /// </summary>
        /// <param name="notificationName"></param>
        /// <returns>whether a Command is currently registered for the given <c>notificationName</c>.</returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasCommand(string notificationName)
        {
            lock (_syncRoot)
            {
                return _commandMap.ContainsKey(notificationName);
            }
        }

        /// <summary>
        /// Remove a previously registered <c>ICommand</c> to <c>INotification</c> mapping.
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to remove the <c>ICommand</c> mapping for</param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RemoveCommand(string notificationName)
        {
            lock (_syncRoot)
            {
                if (_commandMap.ContainsKey(notificationName))
                {
                    // remove the observer

                    // This call needs to be monitored carefully. Have to make sure that RemoveObserver
                    // doesn't call back into the controller, or a dead lock could happen.
                    _view.RemoveObserver(notificationName, this);
                    _commandMap.Remove(notificationName);
                }
            }
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Singleton Factory method.  This method is thread safe.
        /// </summary>
        public static IController Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Controller();
                        }
                    }
                }

                return _Instance;
            }
        }

        #endregion

        #region Protected & Internal Methods

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static Controller()
        {
        }

        /// <summary>
        /// Initialize the Singleton <c>Controller</c> instance
        /// </summary>
        /// <remarks>
        ///     <para>Called automatically by the constructor</para>
        ///     
        ///     <para>
        ///         Note that if you are using a subclass of <c>View</c>
        ///         in your application, you should also subclass <c>Controller</c>
        ///         and override the <c>initializeController</c> method in the following way:
        ///     </para>
        /// 
        ///     <c>
        ///         // ensure that the Controller is talking to my IView implementation
        ///         public override void initializeController()
        ///         {
        ///             view = MyView.Instance;
        ///         }
        ///     </c>
        /// </remarks>
        protected virtual void InitializeController()
        {
            _view = View.Instance;
        }

        #endregion

        #region Members

        /// <summary>
        /// Local reference to View
        /// </summary>
        protected IView _view;

        /// <summary>
        /// Mapping of Notification names to Command Class references
        /// </summary>
        protected IDictionary<string, Type> _commandMap;

        /// <summary>
        /// Singleton instance, can be sublcassed though....
        /// </summary>
		protected static volatile IController _Instance;

        /// <summary>
        /// Used for locking
        /// </summary>
        protected readonly object _syncRoot = new object();

        /// <summary>
        /// Used for locking the instance calls
        /// </summary>
        protected static readonly object _SyncRoot = new object();

        #endregion
    }
}
