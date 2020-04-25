// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "DefaultCommands" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;

namespace K.Console
{
    /// <summary>
    /// 帮助指令
    /// </summary>
    public static class HelpCommand
    {
        public static readonly string Name = "HELP";
        public static readonly string Description = "Display the list of available commands or details about a specific command.";
        public static readonly string Specification = "HELP [command]";

        public static string Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return DisplayAvailableCommands();
            }
            else
            {
                return DisplayCommandDetails(args[0]);
            }
        }

        private static StringBuilder _CommandBuffer = new StringBuilder();

        private static string DisplayAvailableCommands()
        {
            _CommandBuffer.Length = 0; // clear the command list before rebuilding it
            _CommandBuffer.Append("<b>All Commands</b>\n");

            foreach (var command in ConsoleCommand.GetAll())
            {
                _CommandBuffer.Append(string.Format("\t<b><color=#00FF00>{0}</color></b> - {1}\n", command.name, command.description));
            }

            _CommandBuffer.Append("Display details about specific command, 'HELP' followed by the command name.");
            return _CommandBuffer.ToString();
        }

        private static string DisplayCommandDetails(string commandName)
        {
            string formatting = "<b>{0} Command</b>\n<b>Description:</b> {1}\n<b>Specification:</b> {2}";

            try
            {
                var command = ConsoleCommand.GetCommand(commandName);
                return string.Format(formatting, command.name, command.description, command.specification);
            }
            catch (Exception exception)
            {
                return string.Format("Cannot find help information about {0}.", exception.Message);
            }
        }
    }

    /// <summary>
    /// 清屏指令
    /// </summary>
    public static class ClearCommand
    {
        public static readonly string Name = "CLEAR";
        public static readonly string Description = "Clear Console";
        public static readonly string Specification = "clear";

        public static string Execute(string[] args)
        {
            Console.Clear();
            return string.Empty;
        }
    }

    public static class CloseCommand
    {
        public static readonly string Name = "CLOSE";
        public static readonly string Description = "Close Console";
        public static readonly string Specification = "close";

        public static string Execute(string[] args)
        {
            Console.Hide();
            return string.Empty;
        }
    }
}
