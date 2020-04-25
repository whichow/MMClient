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
    /// A base <c>INotification</c> implementation
    /// </summary>
    public class Notification : INotification
    {
        #region Constructors

        /// <summary>
        /// Constructs a new notification with the specified name, default body and type
        /// </summary>
        /// <param name="name">The name of the <c>Notification</c> instance</param>
        public Notification(string name)
            : this(name, null, null)
        { }

        /// <summary>
        /// Constructs a new notification with the specified name and body, with the default type
        /// </summary>
        /// <param name="name">The name of the <c>Notification</c> instance</param>
        /// <param name="body">The <c>Notification</c>s body</param>
        public Notification(string name, object body)
            : this(name, body, null)
        { }

        /// <summary>
        /// Constructs a new notification with the specified name, body and type
        /// </summary>
        /// <param name="name">The name of the <c>Notification</c> instance</param>
        /// <param name="body">The <c>Notification</c>s body</param>
        /// <param name="type">The type of the <c>Notification</c></param>
        public Notification(string name, object body, string type)
        {
            _name = name;
            _body = body;
            _type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the string representation of the <c>Notification instance</c>
        /// </summary>
        /// <returns>The string representation of the <c>Notification</c> instance</returns>
        public override string ToString()
        {
            string msg = "Notification Name: " + name;
            msg += "\nBody:" + ((body == null) ? "null" : body.ToString());
            msg += "\nType:" + ((type == null) ? "null" : type);
            return msg;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// The name of the <c>Notification</c> instance
        /// </summary>
        public virtual string name
        {
            get { return _name; }
        }

        /// <summary>
        /// The body of the <c>Notification</c> instance
        /// </summary>
        /// <remarks>This accessor is thread safe</remarks>
        public virtual object body
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return _body;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                _body = value;
            }
        }

        /// <summary>
        /// The type of the <c>Notification</c> instance
        /// </summary>
        /// <remarks>This accessor is thread safe</remarks>
        public virtual string type
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return _type;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                _type = value;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// The name of the notification instance 
        /// </summary>
        private string _name;

        /// <summary>
        /// The type of the notification instance
        /// </summary>
		private string _type;

        /// <summary>
        /// The body of the notification instance
        /// </summary>
		private object _body;

        #endregion
    }
}
