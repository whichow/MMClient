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
    /// 装饰模式
    /// A base Singleton <c>IFacade</c> implementation
    /// </summary>
    public class Facade : IFacade
    {
        #region Constructors

        /// <summary>
        /// Constructor that initializes the Facade
        /// </summary>
        /// <remarks>
        ///     <para>This <c>IFacade</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>Facade.Instance</c></para>
        /// </remarks>
        protected Facade()
        {
            InitializeFacade();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            //var allTypes = new List<Type>();

            //var execAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            //if (execAssembly != null)
            //{
            //    allTypes.AddRange(execAssembly.GetTypes());
            //}

            //var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //if (entryAssembly != null && entryAssembly != execAssembly)
            //{
            //    allTypes.AddRange(entryAssembly.GetTypes());
            //}

            //var callAssembly = System.Reflection.Assembly.GetCallingAssembly();
            //if (callAssembly != null && callAssembly != execAssembly && callAssembly != entryAssembly)
            //{
            //    allTypes.AddRange(callAssembly.GetTypes());
            //}

            //var commandType = typeof(Command);
            //var proxyType = typeof(Proxy);
            //var mediatorType = typeof(Mediator);

            //foreach (var type in allTypes)
            //{
            //    if (type.IsSubclassOf(commandType))
            //    {
            //        var customAttributes = type.GetCustomAttributes(typeof(NotifierAttribute), false);
            //        if (customAttributes != null)
            //        {
            //            foreach (var customAttribute in customAttributes)
            //            {
            //                string notificationName = ((NotifierAttribute)customAttribute).notification;
            //                this.RegisterCommand(notificationName, type);
            //            }
            //        }
            //        continue;
            //    }

            //    if (type.IsSubclassOf(proxyType))
            //    {
            //        var proxyObj = Activator.CreateInstance(proxyType) as IProxy;
            //        this.RegisterProxy(proxyObj);
            //        UnityEngine.Debug.Log("proxyObj" + (proxyObj == null) + proxyObj.proxyName);
            //        continue;
            //    }

            //    if (type.IsSubclassOf(mediatorType))
            //    {
            //        var mediatorObj = Activator.CreateInstance(mediatorType) as IMediator;
            //        this.RegisterMediator(mediatorObj);
            //        continue;
            //    }
            //}
        }

        #endregion

        #region IFacade Members

        #region Proxy

        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c> by name
        /// </summary>
        /// <param name="proxy">The <c>IProxy</c> to be registered with the <c>Model</c></param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RegisterProxy(IProxy proxy)
        {
            // The model is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the model.
            _model.RegisterProxy(proxy);
        }

        /// <summary>
        /// Retrieve a <c>IProxy</c> from the <c>Model</c> by name
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> instance to be retrieved</param>
        /// <returns>The <c>IProxy</c> previously regisetered by <c>proxyName</c> with the <c>Model</c></returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IProxy GetProxy(string proxyName)
        {
            // The model is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the model.
            return _model.GetProxy(proxyName);
        }

        /// <summary>
        /// Remove an <c>IProxy</c> instance from the <c>Model</c> by name
        /// </summary>
        /// <param name="proxyName">The <c>IProxy</c> to remove from the <c>Model</c></param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IProxy RemoveProxy(string proxyName)
        {
            // The model is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the model.
            return _model.RemoveProxy(proxyName);
        }

        /// <summary>
        /// Check if a Proxy is registered
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> instance to check for</param>
        /// <returns>whether a Proxy is currently registered with the given <c>proxyName</c>.</returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasProxy(string proxyName)
        {
            // The model is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the model.
            return _model.HasProxy(proxyName);
        }

        #endregion

        #region Command

        /// <summary>
        /// Register an <c>ICommand</c> with the <c>Controller</c>
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to associate the <c>ICommand</c> with.</param>
        /// <param name="commandType">A reference to the <c>Type</c> of the <c>ICommand</c></param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RegisterCommand(string notificationName, Type commandType)
        {
            // The controller is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the controller.
            _controller.RegisterCommand(notificationName, commandType);
        }

        /// <summary>
        /// Remove a previously registered <c>ICommand</c> to <c>INotification</c> mapping from the Controller.
        /// </summary>
        /// <param name="notificationName">TRemove a previously registered <c>ICommand</c> to <c>INotification</c> mapping from the Controller.</param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RemoveCommand(string notificationName)
        {
            // The controller is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the controller.
            _controller.RemoveCommand(notificationName);
        }

        /// <summary>
        /// Check if a Command is registered for a given Notification 
        /// </summary>
        /// <param name="notificationName">The name of the <c>INotification</c> to check for.</param>
        /// <returns>whether a Command is currently registered for the given <c>notificationName</c>.</returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasCommand(string notificationName)
        {
            // The controller is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the controller.
            return _controller.HasCommand(notificationName);
        }

        #endregion

        #region Mediator

        /// <summary>
        /// Register an <c>IMediator</c> instance with the <c>View</c>
        /// </summary>
        /// <param name="mediator">A reference to the <c>IMediator</c> instance</param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RegisterMediator(IMediator mediator)
        {
            // The view is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the view.
            _view.RegisterMediator(mediator);
        }

        /// <summary>
        /// Retrieve an <c>IMediator</c> instance from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to retrieve</param>
        /// <returns>The <c>IMediator</c> previously registered with the given <c>mediatorName</c></returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IMediator GetMediator(string mediatorName)
        {
            // The view is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the view.
            return _view.GetMediator(mediatorName);
        }

        /// <summary>
        /// Remove a <c>IMediator</c> instance from the <c>View</c>
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to be removed</param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            // The view is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the view.
            return _view.RemoveMediator(mediatorName);
        }

        /// <summary>
        /// Check if a Mediator is registered or not
        /// </summary>
        /// <param name="mediatorName">The name of the <c>IMediator</c> instance to check for</param>
        /// <returns>whether a Mediator is registered with the given <code>mediatorName</code>.</returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasMediator(string mediatorName)
        {
            // The view is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the view.
            return _view.HasMediator(mediatorName);
        }

        #endregion

        #region Observer

        /// <summary>
        /// Notify <c>Observer</c>s of an <c>INotification</c>
        /// </summary>
        /// <remarks>This method is left public mostly for backward compatibility, and to allow you to send custom notification classes using the facade.</remarks>
        /// <remarks>Usually you should just call sendNotification and pass the parameters, never having to construct the notification yourself.</remarks>
        /// <param name="notification">The <c>INotification</c> to have the <c>View</c> notify observers of</param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void NotifyObservers(INotification notification)
        {
            // The view is initialized in the constructor of the singleton, so this call should be thread safe.
            // This method is thread safe on the view.
            _view.NotifyObservers(notification);
        }

        #endregion

        #endregion

        #region INotifier Members

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notiification to send</param>
        /// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void SendNotification(string notificationName)
        {
            NotifyObservers(new Notification(notificationName));
        }

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
        /// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void SendNotification(string notificationName, object body)
        {
            NotifyObservers(new Notification(notificationName, body));
        }

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
        /// <param name="type">The type of the notification</param>
        /// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void SendNotification(string notificationName, object body, string type)
        {
            NotifyObservers(new Notification(notificationName, body, type));
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Facade Singleton Factory method.  This method is thread safe.
        /// </summary>
        public static Facade Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Facade();
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
        ///</summary>
        static Facade()
        {
        }

        /// <summary>
        /// Initialize the Singleton <c>Facade</c> instance
        /// </summary>
        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }

        /// <summary>
        /// Initialize the <c>Controller</c>
        /// </summary>
		protected virtual void InitializeController()
        {
            if (_controller != null) return;
            _controller = Controller.Instance;
        }

        /// <summary>
        /// Initialize the <c>Model</c>
        /// </summary>
        protected virtual void InitializeModel()
        {
            if (_model != null) return;
            _model = Model.Instance;
        }

        /// <summary>
        /// Initialize the <c>View</c>
        /// </summary>
        protected virtual void InitializeView()
        {
            if (_view != null) return;
            _view = View.Instance;
        }

        #endregion

        #region Members

        /// <summary>
        /// Private reference to the Controller
        /// </summary>
        protected IController _controller;

        /// <summary>
        /// Private reference to the Model
        /// </summary>
        protected IModel _model;

        /// <summary>
        /// Private reference to the View
        /// </summary>
        protected IView _view;

        /// <summary>
        /// The Singleton Facade Instance
        /// </summary>
        protected static volatile Facade _Instance;

        /// <summary>
        /// Used for locking the instance calls
        /// </summary>
        protected static readonly object _SyncRoot = new object();

        #endregion
    }
}
