// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ConsoleCommand" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace K.Console
{
    public struct ConsoleCommand
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string name
        {
            get;
            private set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string description
        {
            get;
            private set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string specification
        {
            get;
            private set;
        }

        /// <summary>
        /// 执行器
        /// </summary>
        public Func<string[], string> executor
        {
            get;
            private set;
        }

        public ConsoleCommand(string name, string description, string specification, Func<string[], string> callback)
        {
            this.name = name;
            this.description = description;
            this.specification = specification;
            this.executor = callback;
        }

        #region Static

        private static Dictionary<string, ConsoleCommand> _RegisterCommands = new Dictionary<string, ConsoleCommand>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Return all the commands in alphabetical order.
        /// </summary>
        public static ConsoleCommand[] GetAll()
        {
            var commands = new ConsoleCommand[_RegisterCommands.Count];
            _RegisterCommands.Values.CopyTo(commands, 0);
            return commands;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="specification"></param>
        /// <param name="callback"></param>
        public static void RegisterCommand(string name, string description, string specification, Func<string[], string> callback)
        {
            _RegisterCommands[name] = new ConsoleCommand(name, description, specification, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ExecuteCommand(string name, string[] args)
        {
            try
            {
                var command = GetCommand(name);

                if (command.executor != null)
                {
                    return command.executor(args);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ConsoleCommand GetCommand(string name)
        {
            ConsoleCommand command;
            if (_RegisterCommands.TryGetValue(name, out command))
            {
                return command;
            }
            else
            {
                name = name.ToUpper();
                throw new Exception("Command[" + name + "] not found.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasCommand(string name)
        {
            return _RegisterCommands.ContainsKey(name);
        }

        #endregion
    }
}