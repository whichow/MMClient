//// ***********************************************************************
//// Assembly         : Unity
//// Author           : kimch
//// Created          : 
////
//// Last Modified By : kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "KLog" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using UnityEngine;
//using System;
//using System.Text;

//namespace Game
//{
//    public static class KLog
//    {
//        public static void Clear()
//        {
//#if DEBUG_MY
//            Debug.ClearDeveloperConsole();
//#endif
//        }

//        public static void Log(object message)
//        {
//#if DEBUG_MY
//            //K.Console.Console.Log(string.Format("{0} [log] {1}", DateTime.Now.ToShortTimeString(), message.ToString()));
//            //KLog.Append(DateTime.Now.ToShortTimeString());
//            //KLog.Append(" [log] ");
//            //KLog.AppendLine(message.ToString());
//            //SystemInfoHelper.log(0, message.ToString());
//#endif
//        }

//        public static void LogError(object message)
//        {
//#if DEBUG_MY
//            //K.Console.Console.Log(string.Format("{0} [error] {1}", DateTime.Now.ToShortTimeString(), message.ToString()));
//            //KLog.Append(DateTime.Now.ToShortTimeString());
//            //KLog.Append(" [error] ");
//            //KLog.AppendLine(message.ToString());
//            //SystemInfoHelper.log(2, message.ToString());
//#endif
//        }

//        public static void LogWarning(object message)
//        {
//#if DEBUG_MY
//            //K.Console.Console.Log(string.Format("{0} [warning] {1}", DateTime.Now.ToShortTimeString(), message.ToString()));
//            //KLog.Append(DateTime.Now.ToShortTimeString());
//            //KLog.Append(" [warning] ");
//            //KLog.AppendLine(message.ToString());
//            //SystemInfoHelper.log(1, message.ToString());
//#endif
//        }

//        public static void LogCallback(string message, string stackTrace, LogType type)
//        {
//            switch (type)
//            {
//                case LogType.Exception:
//                    KLog.Append(DateTime.Now.ToShortTimeString());
//                    KLog.Append(" [exception] ");
//                    KLog.AppendLine(message);
//                    KLog.AppendLine(stackTrace);
//                    break;
//                case LogType.Log:
//                    Log(message);
//                    break;
//                case LogType.Warning:
//                    LogWarning(message);
//                    break;
//                default:
//                    LogError(message);
//                    break;
//            }
//        }


//        #region SHOWLOG

//        private static string _logText = "";
//        private static StringBuilder _logSb = new StringBuilder();

//        private static void Append(string value)
//        {
//            _logSb.Append(value);
//        }

//        private static void AppendLine(string value)
//        {
//            _logSb.AppendLine(value);
//            int len = _logSb.Length - 1;
//            for (int i = len, j = 0; i > 0; i--)
//            {
//                if (_logSb[i] == '\n' && ++j > 3)
//                {
//                    _logText = _logSb.ToString(i + 1, len - i);
//                    return;
//                }
//            }
//            _logText = _logSb.ToString();
//        }

//        public static string GetText()
//        {
//            return _logText;
//        }

//        public static void Save()
//        {
//            if (_logSb.Length == 0)
//            {
//                return;
//            }

//            var fileName = Application.persistentDataPath + "/log.txt";
//            var fileInfo = new System.IO.FileInfo(fileName);
//            // This text is added only once to the file.
//            if (!fileInfo.Exists || fileInfo.Length > 1024 * 1024)
//            {
//                //Create a file to write to.
//                using (var sw = fileInfo.CreateText())
//                {
//                    sw.WriteLine(_logSb.ToString());
//                }
//            }
//            else
//            {
//                // This text will always be added, making the file longer over time
//                // if it is not deleted.
//                using (var sw = fileInfo.AppendText())
//                {
//                    sw.WriteLine(_logSb.ToString());
//                }
//            }

//            _logSb.Length = 0;
//        }

//        #endregion
//    }
//}