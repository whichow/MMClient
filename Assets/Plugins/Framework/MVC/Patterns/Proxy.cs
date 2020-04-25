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
    /// A base <c>IProxy</c> implementation
    /// </summary>
    public class Proxy : Notifier, IProxy
    {
        #region Constants

        /// <summary>
        /// The default proxy name
        /// </summary>
        public static string NAME = "Proxy";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new proxy with the default name and no data
        /// </summary>
        public Proxy()
            : this(NAME, null)
        {
        }

        /// <summary>
        /// Constructs a new proxy with the specified name and no data
        /// </summary>
        /// <param name="proxyName">The name of the proxy</param>
        public Proxy(string proxyName)
            : this(proxyName, null)
        {
        }

        /// <summary>
        /// Constructs a new proxy with the specified name and data
        /// </summary>
        /// <param name="proxyName">The name of the proxy</param>
        /// <param name="data">The data to be managed</param>
		public Proxy(string proxyName, object data)
        {

            _proxyName = (proxyName != null) ? proxyName : NAME;
            if (data != null) _data = data;
        }

        #endregion

        #region IProxy Members 

        /// <summary>
        /// Called by the Model when the Proxy is registered
        /// </summary>
        public virtual void OnRegister()
        {
        }

        /// <summary>
        /// Called by the Model when the Proxy is removed
        /// </summary>
        public virtual void OnRemove()
        {
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Get the proxy name
        /// </summary>
        /// <returns></returns>
        public virtual string proxyName
        {
            get { return _proxyName; }
        }

        /// <summary>
        /// Set the data object
        /// </summary>
        public virtual object userData
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Members

        /// <summary>
        /// The name of the proxy
        /// </summary>
        protected string _proxyName;

        /// <summary>
        /// The data object to be managed
        /// </summary>
        protected object _data;

        #endregion
    }
}
