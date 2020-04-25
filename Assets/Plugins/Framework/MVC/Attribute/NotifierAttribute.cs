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
    /// 通知人属性
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    sealed class NotifierAttribute : System.Attribute
    {
        // See the attribute guidelines at 
        readonly string _notification;

        // This is a positional argument
        public NotifierAttribute(string notification)
        {
            _notification = notification;
        }

        public string notification
        {
            get { return _notification; }
        }
    }
}

