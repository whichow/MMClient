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
    /// 模型
    /// A Singleton <c>IModel</c> implementation
    /// </summary>
    public class Model : IModel
    {
        #region Constructors

        /// <summary>
        /// Constructs and initializes a new model
        /// </summary>
        /// <remarks>
        ///     <para>This <c>IModel</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>Model.getInstance()</c></para>
        /// </remarks>
        protected Model()
        {
            _proxyMap = new Dictionary<string, IProxy>();
            InitializeModel();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c>
        /// </summary>
        /// <param name="proxy">An <c>IProxy</c> to be held by the <c>Model</c></param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual void RegisterProxy(IProxy proxy)
        {
            lock (_syncRoot)
            {
                _proxyMap[proxy.proxyName] = proxy;
            }

            proxy.OnRegister();
        }

        /// <summary>
        /// Get an <c>IProxy</c> from the <c>Model</c>
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> to retrieve</param>
        /// <returns>The <c>IProxy</c> instance previously registered with the given <c>proxyName</c></returns>
        public virtual IProxy GetProxy(string proxyName)
        {
            lock (_syncRoot)
            {
                IProxy proxy;
                _proxyMap.TryGetValue(proxyName, out proxy);
                return proxy;
            }
        }

        /// <summary>
        /// Check if a Proxy is registered
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns>whether a Proxy is currently registered with the given <c>proxyName</c>.</returns>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual bool HasProxy(string proxyName)
        {
            lock (_syncRoot)
            {
                return _proxyMap.ContainsKey(proxyName);
            }
        }

        /// <summary>
        /// Remove an <c>IProxy</c> from the <c>Model</c>
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> instance to be removed</param>
        /// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
        public virtual IProxy RemoveProxy(string proxyName)
        {
            IProxy proxy = null;

            lock (_syncRoot)
            {
                proxy = GetProxy(proxyName);
                if (proxy != null)
                {
                    _proxyMap.Remove(proxyName);
                }
            }

            if (proxy != null)
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// <c>Model</c> Singleton Factory method.  This method is thread safe.
        /// </summary>
        public static IModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Model();
                        }
                    }
                } 
                return _Instance;
            }
        }

        #endregion

        #region Protected & Internal Methods

        /// <summary>
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static Model()
        {
        }

        /// <summary>
        /// Initialize the Singleton <c>Model</c> instance.
        /// </summary>
        /// <remarks>
        ///     <para>Called automatically by the constructor, this is your opportunity to initialize the Singleton instance in your subclass without overriding the constructor</para>
        /// </remarks>
        protected virtual void InitializeModel()
        {
        }

        #endregion

        #region Members

        /// <summary>
        /// Mapping of proxyNames to <c>IProxy</c> instances
        /// </summary>
        protected IDictionary<string, IProxy> _proxyMap;

        /// <summary>
        /// Singleton instance
        /// </summary>
        protected static volatile IModel _Instance;

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
