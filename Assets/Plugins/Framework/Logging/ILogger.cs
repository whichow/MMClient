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
    public interface ILogger
    {
        bool logEnabled
        {
            get;
            set;
        }

        LogType filterLogType
        {
            get;
            set;
        }

        ILogHandler logHandler
        {
            get;
            set;
        }

        bool IsLogTypeAllowed(LogType logType);

        void Log(LogType logType, object message);

        void Log(LogType logType, string tag, object message);
           
        void Log(object message);

        void Log(string tag, object message); 

        void LogWarning(string tag, object message); 

        void LogError(string tag, object message); 

        void LogFormat(LogType logType, string format, params object[] args);

        void LogException(Exception exception);
    }
}
