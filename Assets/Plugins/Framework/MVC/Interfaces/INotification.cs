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
    /// 通知
    /// The interface definition for a MVC Notification
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// The name of the <c>INotification</c> instance
        /// </summary>
        /// <remarks>No setter, should be set by constructor only</remarks>
		string name { get; }

        /// <summary>
        /// The body of the <c>INotification</c> instance
        /// </summary>
		object body { get; set; }

        /// <summary>
        /// The type of the <c>INotification</c> instance
        /// </summary>
        string type { get; set; }

        /// <summary>
        /// Get the string representation of the <c>INotification</c> instance
        /// </summary>
        /// <returns>The string representation of the <c>INotification</c> instance</returns>
        string ToString();
    }
}
