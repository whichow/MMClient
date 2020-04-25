using Game;
using System;
using System.Collections.Generic;

namespace Framework
{
    public delegate void EventAction(IEventData args);

    public class EventDispatcher
    {
        private Dictionary<string, Delegate> _dictAllEvents = new Dictionary<string, Delegate>();
        //public string m_name { get; private set; }

        public EventDispatcher()
        {
        }

        public void RemoveAllEvent()
        {
            if (_dictAllEvents != null)
                _dictAllEvents.Clear();
        }

        #region register event;

        public void AddEvent(string eventType, EventAction method)
        {
            Delegate delegateEvent = null;
            if (_dictAllEvents.ContainsKey(eventType))
            {
                delegateEvent = _dictAllEvents[eventType];
            }
            _dictAllEvents[eventType] = (EventAction)Delegate.Combine((EventAction)delegateEvent, method);
        }

        public void AddEvent(string eventType, Action method)
        {
            Delegate delegateEvent = null;
            if (_dictAllEvents.ContainsKey(eventType))
            {
                delegateEvent = _dictAllEvents[eventType];
            }
            _dictAllEvents[eventType] = (Action)Action.Combine((Action)delegateEvent, method);
        }

        //public void AddEvent<T>(string eventType, Action<T> method)
        //{
        //    Delegate delegateEvent = null;
        //    if (_dictAllEvents.ContainsKey(eventType))
        //    {
        //        delegateEvent = _dictAllEvents[eventType];
        //    }
        //    _dictAllEvents[eventType] = (Action<T>)Delegate.Combine((Action<T>)delegateEvent, method);
        //}

        //public void AddEvent<T, U>(string eventType, Action<T, U> method)
        //{
        //    Delegate delegateEvent = null;
        //    if (_dictAllEvents.ContainsKey(eventType))
        //    {
        //        delegateEvent = _dictAllEvents[eventType];
        //    }
        //    _dictAllEvents[eventType] = (Action<T, U>)Delegate.Combine((Action<T, U>)delegateEvent, method);
        //}

        //public void AddEvent<T, U, K>(string eventType, Action<T, U, K> method)
        //{
        //    Delegate delegateEvent = null;
        //    if (_dictAllEvents.ContainsKey(eventType))
        //    {
        //        delegateEvent = _dictAllEvents[eventType];
        //    }
        //    _dictAllEvents[eventType] = (Action<T, U, K>)Delegate.Combine((Action<T, U, K>)delegateEvent, method);
        //}

        #endregion

        #region unregister event;

        public void RemoveEvent(string eventType, EventAction method)
        {
            if (!HasEvent(eventType))
                return;
            _dictAllEvents[eventType] = (EventAction)Delegate.Remove((EventAction)_dictAllEvents[eventType], method);
        }

        public void RemoveEvent(string eventType, Action method)
        {
            if (!HasEvent(eventType))
                return;
            _dictAllEvents[eventType] = (Action)Delegate.Remove((Action)_dictAllEvents[eventType], method);
        }

        //public void RemoveEvent<T>(string eventType, Action<T> method)
        //{
        //    if (!HasEvent(eventType))
        //        return;
        //    _dictAllEvents[eventType] = (Action<T>)Delegate.Remove((Action<T>)_dictAllEvents[eventType], method);
        //}

        //public void RemoveEvent<T, U>(string eventType, Action<T, U> method)
        //{
        //    if (!HasEvent(eventType))
        //        return;
        //    _dictAllEvents[eventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)_dictAllEvents[eventType], method);
        //}

        //public void RemoveEvent<T, U, K>(string eventType, Action<T, U, K> method)
        //{
        //    if (!HasEvent(eventType))
        //        return;
        //    _dictAllEvents[eventType] = (Action<T, U, K>)Delegate.Remove((Action<T, U, K>)_dictAllEvents[eventType], method);
        //}

        #endregion;

        public bool HasEvent(string eventType)
        {
            if (_dictAllEvents.ContainsKey(eventType) && _dictAllEvents[eventType] == null)
            {
                return false;
            }
            return _dictAllEvents.ContainsKey(eventType);
        }

        #region trigger event;

        public void DispatchEvent(string eventType, IEventData args)
        {
            if (!HasEvent(eventType)) return;

            Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
            for (int i = 0; i < allDelegate.Length; i++)
            {
                if (allDelegate[i] != null)
                {
                    if (allDelegate[i].GetType() == typeof(Action))
                    {
                        ((Action)allDelegate[i]).Invoke();
                    }
                    else if (allDelegate[i].GetType() == typeof(EventAction))
                    {
                        ((EventAction)allDelegate[i]).Invoke(args);
                    }
                    else
                    {
                        Debuger.LogError("[DispatchEvent] not found delegate type " + eventType);
                    }
                }
            }
        }

        public void DispatchEvent(string eventType)
        {
            DispatchEvent(eventType, null);
        }

        //public void DispatchEvent<T>(string eventType, T p1)
        //{
        //    if (!HasEvent(eventType))
        //        return;
        //    Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        //    Action<T> method;
        //    for (int i = 0; i < allDelegate.Length; i++)
        //    {
        //        if (allDelegate[i].GetType() != typeof(Action<T>))
        //            continue;

        //        method = (Action<T>)allDelegate[i];
        //        if (method != null)
        //            method.Invoke(p1);
        //    }
        //}

        //public void DispatchEvent<T, U>(string eventType, T p1, U p2)
        //{
        //    if (!HasEvent(eventType))
        //        return;
        //    Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        //    Action<T, U> method;
        //    for (int i = 0; i < allDelegate.Length; i++)
        //    {
        //        if (allDelegate[i].GetType() != typeof(Action<T, U>))
        //            continue;

        //        method = (Action<T, U>)allDelegate[i];
        //        if (method != null)
        //            method.Invoke(p1, p2);
        //    }
        //}

        //public void DispatchEvent<T, U, K>(string eventType, T p1, U p2, K p3)
        //{
        //    if (!HasEvent(eventType))
        //        return;
        //    Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        //    Action<T, U, K> method;
        //    for (int i = 0; i < allDelegate.Length; i++)
        //    {
        //        if (allDelegate[i].GetType() != typeof(Action<T, U, K>))
        //            continue;

        //        method = (Action<T, U, K>)allDelegate[i];
        //        if (method != null)
        //            method.Invoke(p1, p2, p3);
        //    }
        //}

        #endregion
    }
}
