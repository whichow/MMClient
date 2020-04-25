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
    /// 调用
    /// </summary>
    internal class Invocation
    {
        private event Action _delegate;

        #region Constructors

        public Invocation()
        {
        }

        public Invocation(Action action)
        {
            _delegate += action;
        }

        public Invocation(Type type, object target, string method)
        {
            _delegate += (Action)Delegate.CreateDelegate(type, target, method);
        }

        #endregion

        #region Public Methods

        public void Invoke()
        {
            EventInvoker.Invoke(_delegate);
        }

        public void InvokeAsync()
        {
            EventInvoker.Invoke(_delegate);
        }

        public void Add(Action action)
        {
            _delegate += action;
        }

        public void Remove(Action action)
        {
            _delegate -= action;
        }

        public void RemoveAll()
        {
            _delegate = null;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Invocation<T>
    {
        private event Action<T> _delegate;

        #region Constructors

        public Invocation()
        {
        }

        public Invocation(Action<T> action)
        {
            _delegate += action;
        }

        public Invocation(Type type, object target, string method)
        {
            _delegate += (Action<T>)Delegate.CreateDelegate(type, target, method);
        }

        #endregion

        #region Public Methods

        public void Invoke(T arg)
        {
            EventInvoker.Invoke(_delegate, arg);
        }

        public void InvokeAsync(T arg)
        {
            EventInvoker.Invoke(_delegate, arg);
        }

        public void Add(Action<T> action)
        {
            _delegate += action;
        }

        public void Remove(Action<T> action)
        {
            _delegate -= action;
        }

        public void RemoveAll()
        {
            _delegate = null;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal class Invocation<T1, T2>
    {
        private event Action<T1, T2> _delegate;

        #region Constructors

        public Invocation()
        {
        }

        public Invocation(Action<T1, T2> action)
        {
            _delegate += action;
        }

        public Invocation(Type type, object target, string method)
        {
            _delegate += (Action<T1, T2>)Delegate.CreateDelegate(type, target, method);
        }

        #endregion

        #region Public Methods

        public void Invoke(T1 arg1, T2 arg2)
        {
            EventInvoker.Invoke(_delegate, arg1, arg2);
        }

        public void InvokeAsync(T1 arg1, T2 arg2)
        {
            EventInvoker.Invoke(_delegate, arg1, arg2);
        }

        public void Add(Action<T1, T2> action)
        {
            _delegate += action;
        }

        public void Remove(Action<T1, T2> action)
        {
            _delegate -= action;
        }

        public void RemoveAll()
        {
            _delegate = null;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    internal class Invocation<T1, T2, T3>
    {
        private event Action<T1, T2, T3> _delegate;

        #region Constructors

        public Invocation()
        {
        }

        public Invocation(Action<T1, T2, T3> action)
        {
            _delegate += action;
        }

        public Invocation(Type type, object target, string method)
        {
            _delegate += (Action<T1, T2, T3>)Delegate.CreateDelegate(type, target, method);
        }

        #endregion

        #region Public Methods

        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            EventInvoker.Invoke(_delegate, arg1, arg2, arg3);
        }

        public void InvokeAsync(T1 arg1, T2 arg2, T3 arg3)
        {
            EventInvoker.Invoke(_delegate, arg1, arg2, arg3);
        }

        public void Add(Action<T1, T2, T3> action)
        {
            _delegate += action;
        }

        public void Remove(Action<T1, T2, T3> action)
        {
            _delegate -= action;
        }

        public void RemoveAll()
        {
            _delegate = null;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    internal class Invocation<T1, T2, T3, T4>
    {
        private event Action<T1, T2, T3, T4> _delegate;

        #region Constructors

        public Invocation()
        {
        }

        public Invocation(Action<T1, T2, T3, T4> action)
        {
            _delegate += action;
        }

        public Invocation(Type type, object target, string method)
        {
            _delegate += (Action<T1, T2, T3, T4>)Delegate.CreateDelegate(type, target, method);
        }

        #endregion

        #region Public Methods

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            EventInvoker.Invoke(_delegate, arg1, arg2, arg3, arg4);
        }

        public void InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            EventInvoker.Invoke(_delegate, arg1, arg2, arg3, arg4);
        }

        public void Add(Action<T1, T2, T3, T4> action)
        {
            _delegate += action;
        }

        public void Remove(Action<T1, T2, T3, T4> action)
        {
            _delegate -= action;
        }

        public void RemoveAll()
        {
            _delegate = null;
        }

        #endregion
    }
}
