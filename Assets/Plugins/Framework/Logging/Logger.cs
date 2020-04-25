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
    public class Logger : ILogger
    {
        private const string kNoTagFormat = "{0}";

        private const string kTagFormat = "{0}: {1}";

        public bool logEnabled
        {
            get;
            set;
        }

        public LogType filterLogType
        {
            get;
            set;
        }

        public ILogHandler logHandler
        {
            get;
            set;
        }

        private Logger()
        {
        }

        public Logger(ILogHandler logHandler)
        {
            this.logEnabled = true;
            this.filterLogType = LogType.Log;
        }

        public bool IsLogTypeAllowed(LogType logType)
        {
            return this.logEnabled && (logType <= this.filterLogType || logType == LogType.Exception);
        }

        private static string GetString(object message)
        {
            return (message == null) ? "Null" : message.ToString();
        }

        public void Log(LogType logType, object message)
        {
            if (this.IsLogTypeAllowed(logType))
            {
                this.logHandler.LogFormat(logType, "{0}",
                    Logger.GetString(message));
            }
        }

        public void Log(LogType logType, string tag, object message)
        {
            if (this.IsLogTypeAllowed(logType))
            {
                this.logHandler.LogFormat(logType, "{0}: {1}",
                    tag,
                    Logger.GetString(message));
            }
        }

        public void Log(object message)
        {
            if (this.IsLogTypeAllowed(LogType.Log))
            {
                this.logHandler.LogFormat(LogType.Log, "{0}",
                    Logger.GetString(message));
            }
        }

        public void Log(string tag, object message)
        {
            if (this.IsLogTypeAllowed(LogType.Log))
            {
                this.logHandler.LogFormat(LogType.Log, "{0}: {1}",
                    tag,
                    Logger.GetString(message));
            }
        }

        public void LogWarning(string tag, object message)
        {
            if (this.IsLogTypeAllowed(LogType.Warning))
            {
                this.logHandler.LogFormat(LogType.Warning, "{0}: {1}",
                    tag,
                    Logger.GetString(message));
            }
        }

        public void LogError(string tag, object message)
        {
            if (this.IsLogTypeAllowed(LogType.Error))
            {
                this.logHandler.LogFormat(LogType.Error, "{0}: {1}",
                    tag,
                    Logger.GetString(message));
            }
        }

        public void LogFormat(LogType logType, string format, params object[] args)
        {
            if (this.IsLogTypeAllowed(logType))
            {
                this.logHandler.LogFormat(logType, format, args);
            }
        }

        public void LogException(Exception exception)
        {
            if (this.logEnabled)
            {
                this.logHandler.LogException(exception);
            }
        }
    }
}
