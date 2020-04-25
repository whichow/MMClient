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
    /// 代理模式
    /// The interface definition for a MVC Proxy
    /// </summary>
    public interface IProxy
    {
        /// <summary>
        /// The Proxy instance name
        /// </summary>
        string proxyName { get; }

        /// <summary>
        /// The data of the proxy
        /// </summary>
		object userData { get; set; }

        /// <summary>
        /// Called by the Model when the Proxy is registered
        /// </summary>
        void OnRegister();

        /// <summary>
        /// Called by the Model when the Proxy is removed
        /// </summary>
        void OnRemove();
    }
}
