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
    /// 命令模式
    /// A base <c>ICommand</c> implementation that executes other <c>ICommand</c>s
    /// </summary>
    public class Command : Notifier, ICommand
    {
        #region Constructors

        /// <summary>
        /// Constructs a new macro command
        /// </summary>
        /// <remarks>
        ///     <para>You should not need to define a constructor, instead, override the <c>initializeMacroCommand</c> method</para>
        ///     <para>If your subclass does define a constructor, be sure to call <c>super()</c></para>
        /// </remarks>
        public Command()
        {
            _subCommands = new List<Type>();
            InitializeMacroCommand();
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Execute this <c>MacroCommand</c>'s <i>SubCommands</i>
        /// </summary>
        /// <param name="notification">The <c>INotification</c> object to be passsed to each <i>SubCommand</i></param>
        public virtual void Execute(INotification notification)
        {
            while (_subCommands.Count > 0)
            {
                Type commandType = _subCommands[0];
                object commandInstance = Activator.CreateInstance(commandType);

                if (commandInstance is ICommand)
                {
                    ((ICommand)commandInstance).Execute(notification);
                }

                _subCommands.RemoveAt(0);
            }
        }

        #endregion

        #region Protected & Internal Methods

        /// <summary>
        /// Initialize the <c>MacroCommand</c>
        /// </summary>
        protected virtual void InitializeMacroCommand()
        {
        }

        /// <summary>
        /// Add a <i>SubCommand</i>
        /// </summary>
        /// <param name="commandType">A a reference to the <c>Type</c> of the <c>ICommand</c></param>
        protected void AddSubCommand(Type commandType)
        {
            _subCommands.Add(commandType);
        }

        #endregion

        #region Members

        private IList<Type> _subCommands;

        #endregion
    }
}
