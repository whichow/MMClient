// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;

namespace K.Events
{
    /// <summary>
    /// 事件(安全)
    /// </summary>
    public class SimpleEvent
    {
        #region Public Methods

        /// <summary>
        /// Invoke all registered callbacks (runtime and persistent).
        /// </summary>
        public void Invoke()
        {
            _invocation.Invoke();
        }

        /// <summary>
        /// Add a non persistent listener to the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Add(Action callback)
        {
            _invocation.Add(callback);
        }

        /// <summary>
        /// Remove a non persistent listener from the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Remove(Action callback)
        {
            _invocation.Remove(callback);
        }

        /// <summary>
        /// Remove all non persistent listener from the SimpleEvent
        /// </summary>
        public void RemoveAll()
        {
            _invocation.RemoveAll();
        }

        #endregion

        #region Members

        /// <summary>
        /// 内部调用(安全)
        /// </summary>
        Invocation _invocation = new Invocation();

        #endregion
    }

    /// <summary>
    /// 事件(安全)
    /// </summary>
    public class SimpleEvent<T>
    {
        #region Public Methods

        /// <summary>
        /// Invoke all registered callbacks (runtime and persistent).
        /// </summary>
        public void Invoke(T arg)
        {
            _invocation.Invoke(arg);
        }

        /// <summary>
        /// Add a non persistent listener to the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Add(Action<T> callback)
        {
            _invocation.Add(callback);
        }

        /// <summary>
        /// Remove a non persistent listener from the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Remove(Action<T> callback)
        {
            _invocation.Remove(callback);
        }

        /// <summary>
        /// Remove all non persistent listener from the SimpleEvent
        /// </summary>
        public void RemoveAll()
        {
            _invocation.RemoveAll();
        }

        #endregion

        #region Members

        /// <summary>
        /// 内部调用(安全)
        /// </summary>
        Invocation<T> _invocation = new Invocation<T>();

        #endregion
    }

    /// <summary>
    /// 事件(安全)
    /// </summary>
    public class SimpleEvent<T1, T2>
    {
        #region Public Methods

        /// <summary>
        /// Invoke all registered callbacks (runtime and persistent).
        /// </summary>
        public void Invoke(T1 arg1, T2 arg2)
        {
            _invocation.Invoke(arg1, arg2);
        }

        /// <summary>
        /// Add a non persistent listener to the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Add(Action<T1, T2> callback)
        {
            _invocation.Add(callback);
        }

        /// <summary>
        /// Remove a non persistent listener from the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Remove(Action<T1, T2> callback)
        {
            _invocation.Remove(callback);
        }

        /// <summary>
        /// Remove all non persistent listener from the SimpleEvent
        /// </summary>
        public void RemoveAll()
        {
            _invocation.RemoveAll();
        }

        #endregion

        #region Members

        /// <summary>
        /// 内部调用(安全)
        /// </summary>
        Invocation<T1, T2> _invocation = new Invocation<T1, T2>();

        #endregion
    }

    /// <summary>
    /// 事件(安全)
    /// </summary>
    public class SimpleEvent<T1, T2, T3>
    {
        #region Public Methods

        /// <summary>
        /// Invoke all registered callbacks (runtime and persistent).
        /// </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            _invocation.Invoke(arg1, arg2, arg3);
        }

        /// <summary>
        /// Add a non persistent listener to the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Add(Action<T1, T2, T3> callback)
        {
            _invocation.Add(callback);
        }

        /// <summary>
        /// Remove a non persistent listener from the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Remove(Action<T1, T2, T3> callback)
        {
            _invocation.Remove(callback);
        }

        /// <summary>
        /// Remove all non persistent listener from the SimpleEvent
        /// </summary>
        public void RemoveAll()
        {
            _invocation.RemoveAll();
        }

        #endregion

        #region Members

        /// <summary>
        /// 内部调用(安全)
        /// </summary>
        Invocation<T1, T2, T3> _invocation = new Invocation<T1, T2, T3>();

        #endregion
    }

    /// <summary>
    /// 事件(安全)
    /// </summary>
    public class SimpleEvent<T1, T2, T3, T4>
    {
        #region Public Methods

        /// <summary>
        /// Invoke all registered callbacks (runtime and persistent).
        /// </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _invocation.Invoke(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Add a non persistent listener to the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Add(Action<T1, T2, T3, T4> callback)
        {
            _invocation.Add(callback);
        }

        /// <summary>
        /// Remove a non persistent listener from the SimpleEvent.
        /// </summary>
        /// <param name="callback"></param>
        public void Remove(Action<T1, T2, T3, T4> callback)
        {
            _invocation.Remove(callback);
        }

        /// <summary>
        /// Remove all non persistent listener from the SimpleEvent
        /// </summary>
        public void RemoveAll()
        {
            _invocation.RemoveAll();
        }

        #endregion

        #region Members

        /// <summary>
        /// 内部调用(安全)
        /// </summary>
        Invocation<T1, T2, T3, T4> _invocation = new Invocation<T1, T2, T3, T4>();

        #endregion
    }
}
