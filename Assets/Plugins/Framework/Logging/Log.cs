// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************

using System;

namespace K
{
    public static class LogUtil
    {
        #region Default LogHandler

        internal sealed class DebugLogHandler : ILogHandler
        {
            public void LogFormat(LogType logType, string format, params object[] args)
            {
            }

            public void LogException(Exception exception)
            {
            }
        }

        #endregion

        internal static ILogger s_Logger = new Logger(new DebugLogHandler());

        public static ILogger logger
        {
            get
            {
                return s_Logger;
            }
        }

        public static void Log(object message)
        {
            logger.Log(LogType.Log, message);
        }

        public static void LogFormat(string format, params object[] args)
        {
            logger.LogFormat(LogType.Log, format, args);
        }

        public static void LogError(object message)
        {
            logger.Log(LogType.Error, message);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            logger.LogFormat(LogType.Error, format, args);
        }

        public static void LogException(Exception exception)
        {
            logger.LogException(exception);
        }

        public static void LogWarning(object message)
        {
            logger.Log(LogType.Warning, message);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            logger.LogFormat(LogType.Warning, format, args);
        }
    }
}
