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
    /// 视图
    /// A Singleton <c>IView</c> implementation.
    /// </summary>
    public class View : IView
    {
        #region Constructors

        /// <summary>
        /// Constructs and initializes a new view
        /// </summary>
        protected View()
        {
            _mediatorMap = new Dictionary<string, IMediator>();
            _observerMap = new Dictionary<string, IList<IObserver>>();
            InitializeView();
        }

        #endregion

        #region Public Methods

        #region Observer

        /// <summary>
        /// Register an <c>IObserver</c> to be notified of <c>INotifications</c> with a given name
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotifications</c> to notify this <c>IObserver</c> of</param>
        /// <param name="observer">The <c>IObserver</c> to register</param>
        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            lock (_syncRoot)
            {
                if (!_observerMap.ContainsKey(notificationName))
                {
                    _observerMap[notificationName] = new List<IObserver>();
                }

                _observerMap[notificationName].Add(observer);
            }
        }

        /// <summary>
        /// Notify the <c>IObservers</c> for a particular <c>INotification</c>
        /// </summary>
        /// <param name="notification">The <c>INotification</c> to notify <c>IObservers</c> of</param>
        public virtual void NotifyObservers(INotification notification)
        {
            IList<IObserver> observers = null;

            lock (_syncRoot)
            {
                if (_observerMap.ContainsKey(notification.name))
                {
                    // Get a reference to the observers list for this notification name
                    var observers_ref = _observerMap[notification.name];
                    // Copy observers from reference array to working array, 
                    // since the reference array may change during the notification loop
                    observers = new List<IObserver>(observers_ref);
                }
            }

            // Notify outside of the lock
            if (observers != null)
            {
                // Notify Observers from the working array				
                for (int i = 0; i < observers.Count; i++)
                {
                    var observer = observers[i];
                    observer.NotifyObserver(notification);
                }
            }
        }

        /// <summary>
        /// Remove the observer for a given notifyContext from an observer list for a given Notification name.
        /// </summary>
        /// <param name="notificationName">which observer list to remove from</param>
        /// <param name="notifyContext">remove the observer with this object as its notifyContext</param>
        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            lock (_syncRoot)
            {
                // the observer list for the notification under inspection
                if (_observerMap.ContainsKey(notificationName))
                {
                    var observers = _observerMap[notificationName];

                    // find the observer for the notifyContext
                    for (int i = 0; i < observers.Count; i++)
                    {
                        if (observers[i].CompareNotifyTarget(notifyContext))
                        {
                            // there can only be one Observer for a given notifyContext 
                            // in any given Observer list, so remove it and break
                            observers.RemoveAt(i);
                            break;
                        }
                    }

                    // Also, when a Notification's Observer list length falls to 
                    // zero, delete the notification key from the observer map
                    if (observers.Count == 0)
                    {
                        _observerMap.Remove(notificationName);
                    }
                }
            }
        }

        #endregion

        #region Mediator

        /// <summary>
        /// Register an <c>IMediator</c> instance with the <c>View</c>
        /// </summary>
        /// <param name="mediator">A reference to the <c>IMediator</c> instance</param>
        public virtual void RegisterMediator(IMediator mediator)
        {
            lock (_syncRoot)
            {
                // do not allow re-registration (you must to removeMediator fist)
                if (_mediatorMap.ContainsKey(mediator.mediatorName)) return;

                // Register the Mediator for retrieval by name
                _mediatorMap.Add(mediator.mediatorName, mediator);

                // Get Notification interests, if any.
                var interests = mediator.ListNotificationInterests();

                // Register Mediator as an observer for each of its notification interests
                if (interests != null && interests.Count > 0)
                {
                    // Create Observer
                    var observer = new Observer(mediator, mediator.HandleNotification);

                    // Register Mediator as Observer for its list of Notification interests
                    for (int i = 0; i < interests.Count; i++)
                    {
                        RegisterObserver(interests[i].ToString(), observer);
                    }
                }
            }

            // alert the mediator that it has been registered
            mediator.OnRegister();
        }

        /// <summary>
        /// Retrieve an <c>IMediator</c> from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to retrieve</param>
        /// <returns>The <c>IMediator</c> instance previously registered with the given <c>mediatorName</c></returns>
        public virtual IMediator GetMediator(string mediatorName)
        {
            lock (_syncRoot)
            {
                IMediator mediator;
                _mediatorMap.TryGetValue(mediatorName, out mediator);
                return mediator;
            }
        }

        /// <summary>
        /// Remove an <c>IMediator</c> from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            IMediator mediator = null;

            lock (_syncRoot)
            {
                // Retrieve the named mediator
                if (!_mediatorMap.TryGetValue(mediatorName, out mediator))
                { return null; }

                // for every notification this mediator is interested in...
                var interests = mediator.ListNotificationInterests();

                if (interests != null && interests.Count > 0)
                {
                    for (int i = 0; i < interests.Count; i++)
                    {
                        // remove the observer linking the mediator 
                        // to the notification interest
                        RemoveObserver(interests[i], mediator);
                    }
                }

                // remove the mediator from the map		
                _mediatorMap.Remove(mediatorName);
            }

            // alert the mediator that it has been removed
            if (mediator != null)
            {
                mediator.OnRemove();
            }
            return mediator;
        }

        /// <summary>
        /// Check if a Mediator is registered or not
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns>whether a Mediator is registered with the given <code>mediatorName</code>.</returns>
        public virtual bool HasMediator(string mediatorName)
        {
            lock (_syncRoot)
            {
                return _mediatorMap.ContainsKey(mediatorName);
            }
        }

        #endregion

        #endregion

        #region Accessors

        /// <summary>
        /// View Singleton Factory method.  This method is thread safe.
        /// </summary>
        public static IView Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new View();
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
        static View()
        {
        }

        /// <summary>
        /// Initialize the Singleton View instance
        /// </summary>
        /// <remarks>
        /// <para>Called automatically by the constructor, this is your opportunity to initialize the Singleton instance in your subclass without overriding the constructor</para>
        /// </remarks>
        protected virtual void InitializeView()
        {
        }

        #endregion

        #region Members

        /// <summary>
        /// Mapping of Mediator names to Mediator instances
        /// </summary>
        protected IDictionary<string, IMediator> _mediatorMap;

        /// <summary>
        /// Mapping of Notification names to Observer lists
        /// </summary>
		protected IDictionary<string, IList<IObserver>> _observerMap;

        /// <summary>
        /// Singleton instance
        /// </summary>
        protected static volatile IView _Instance;

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
