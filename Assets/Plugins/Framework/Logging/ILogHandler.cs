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
    public interface ILogHandler
    {
        void LogFormat(LogType logType, string format, params object[] args);

        void LogException(Exception exception);
    }
}
