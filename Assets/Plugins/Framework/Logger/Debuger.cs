using System.Text;
using UnityEngine;

public class Debuger
{
    public static bool EnableLog = true;

    #region Log

    public static void Log(object message, Object context)
    {
        if (EnableLog)
        {
#if UNITY_EDITOR
            Debug.LogFormat(context, "<color=#00aadd>{0}</color>", message);
#else
            Debug.Log(message, context);
#endif
        }
    }

    public static void Log(object message)
    {
        if (EnableLog)
        {
#if UNITY_EDITOR
            Debug.LogFormat("<color=#00aadd>{0}</color>", message);
#else
            Debug.Log(message);
#endif
        }
    }

    public static void LogFormat(Object context, string format, params object[] args)
    {
        if (EnableLog)
        {
            string message = string.Format(format, args);
            Log(message, context);
        }
    }

    public static void LogFormat(string format, params object[] args)
    {
        if (EnableLog)
        {
            string message = string.Format(format, args);
            Log(message);
        }
    }

    #endregion

    #region Warning

    public static void LogWarning(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.LogWarning(message, context);
        }
    }

    public static void LogWarning(object message)
    {
        if (EnableLog)
        {
            Debug.LogWarning(message);
        }
    }

    public static void LogWarningFormat(Object context, string format, params object[] args)
    {
        if (EnableLog)
        {
            Debug.LogWarningFormat(context, format, args);
        }
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        if (EnableLog)
        {
            Debug.LogWarningFormat(format, args);
        }
    }

    #endregion

    #region Error

    public static void LogError(object message, Object context)
    {
        Debug.LogError(message, context);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message);
    }

    public static void LogErrorFormat(Object context, string format, params object[] args)
    {
        Debug.LogErrorFormat(context, format, args);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }

    #endregion

    public static void LogCallback(string message, string stackTrace, LogType type)
    {
        Append(System.DateTime.Now.ToShortTimeString());
        switch (type)
        {
            case LogType.Error:
                Append(" [error] ");
                break;
            case LogType.Exception:
                Append(" [exception] ");
                break;
            case LogType.Log:
                break;
            case LogType.Warning:
                break;
            default:
                break;
        }
        if (type == LogType.Error || type == LogType.Exception)
        {
            AppendLine(message);
            AppendLine(stackTrace);
            if (_logSb.Length > 1024 * 1024)
            {
                Save();
            }
        }
    }

    #region SaveLog

    //private static string _logText = "";
    private static StringBuilder _logSb = new StringBuilder();

    private static void Append(string value)
    {
        _logSb.Append(value);
    }

    private static void AppendLine(string value)
    {
        _logSb.AppendLine(value);
        //int len = _logSb.Length - 1;
        //for (int i = len, j = 0; i > 0; i--)
        //{
        //    if (_logSb[i] == '\n' && ++j > 3)
        //    {
        //        _logText = _logSb.ToString(i + 1, len - i);
        //        return;
        //    }
        //}
        //_logText = _logSb.ToString();

    }

    //public static string GetText()
    //{
    //    return _logText;
    //}

    public static void Save()
    {
        if (_logSb.Length == 0)
        {
            return;
        }

        var fileName = Application.persistentDataPath + "/log.txt";
        var fileInfo = new System.IO.FileInfo(fileName);
        if (!fileInfo.Exists || fileInfo.Length > 1024 * 1024)
        {
            using (var sw = fileInfo.CreateText())
            {
                sw.WriteLine(_logSb.ToString());
            }
        }
        else
        {
            using (var sw = fileInfo.AppendText())
            {
                sw.WriteLine(_logSb.ToString());
            }
        }
        _logSb.Length = 0;
    }

    #endregion

}
