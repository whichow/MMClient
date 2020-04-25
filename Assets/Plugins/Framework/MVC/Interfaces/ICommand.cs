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
    /// 命令模式
    /// The interface definition for a MVC Command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute the <c>ICommand</c>'s logic to handle a given <c>INotification</c>
        /// </summary>
        /// <param name="notification">An <c>INotification</c> to handle</param>
		void Execute(INotification notification);
    }
}
