// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;

namespace K.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <arg name="action"></arg>
        public static void Invoke(Action action)
        {
            if (action != null)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                    //LogUtil.LogException(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typearg name="T"></typearg>
        /// <arg name="action"></arg>
        /// <arg name="arg"></arg>
        public static void Invoke<T>(Action<T> action, T arg)
        {
            if (action != null)
            {
                try
                {
                    action(arg);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                    //LogUtil.LogException(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typearg name="T1"></typearg>
        /// <typearg name="T2"></typearg>
        /// <arg name="action"></arg>
        /// <arg name="t1"></arg>
        /// <arg name="t2"></arg>
        public static void Invoke<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action != null)
            {
                try
                {
                    action(arg1, arg2);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                    //LogUtil.LogException(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typearg name="T1"></typearg>
        /// <typearg name="T2"></typearg>
        /// <typearg name="T3"></typearg>
        /// <arg name="action"></arg>
        /// <arg name="t1"></arg>
        /// <arg name="t2"></arg>
        /// <arg name="t3"></arg>
        public static void Invoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (action != null)
            {
                try
                {
                    action(arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                    //LogUtil.LogException(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="action"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        public static void Invoke<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (action != null)
            {
                try
                {
                    action(arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                    //LogUtil.LogException(ex);
                }
            }
        }
    }
}