#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： REventDispatcher 
     * 创建人	： roy
     * 创建时间	： 2016/12/19 16:40:17 
     * 描述  	： 事件载体：派发，接收
*/
#endregion

using System;
using System.Collections.Generic;

namespace Plugin
{
    public class REventDispatcher
    {
        private Dictionary<string, Delegate> _dictAllEvents = new Dictionary<string, Delegate>();
        //public string m_name { get; private set; }
        public REventDispatcher()
        {
        }

        public void RemoveAllEvent()
        {
            if (_dictAllEvents != null)
                _dictAllEvents.Clear();
        }

        #region register event;
        public void AddEvent(string eventType, Action method)
        {
            Delegate delegateEvent = null;
            if (_dictAllEvents.ContainsKey(eventType))
            {
                delegateEvent = _dictAllEvents[eventType];
            }
            _dictAllEvents[eventType] = (Action)Delegate.Combine((Action)delegateEvent, method);
        }

        public void AddEvent<T>(string eventType, Action<T> method)
        {
            Delegate delegateEvent = null;
            if (_dictAllEvents.ContainsKey(eventType))
            {
                delegateEvent = _dictAllEvents[eventType];
            }
            _dictAllEvents[eventType] = (Action<T>)Delegate.Combine((Action<T>)delegateEvent, method);
        }

        public void AddEvent<T, U>(string eventType, Action<T, U> method)
        {
            Delegate delegateEvent = null;
            if (_dictAllEvents.ContainsKey(eventType))
            {
                delegateEvent = _dictAllEvents[eventType];
            }
            _dictAllEvents[eventType] = (Action<T, U>)Delegate.Combine((Action<T, U>)delegateEvent, method);
        }

        public void AddEvent<T, U, K>(string eventType, Action<T, U, K> method)
        {
            Delegate delegateEvent = null;
            if (_dictAllEvents.ContainsKey(eventType))
            {
                delegateEvent = _dictAllEvents[eventType];
            }
            _dictAllEvents[eventType] = (Action<T, U, K>)Delegate.Combine((Action<T, U, K>)delegateEvent, method);
        }

        #endregion

        #region unregister event;
        public void RemoveEvent(string eventType, Action method)
        {
            if (!HasEvent(eventType))
                return;
            _dictAllEvents[eventType] = (Action)Delegate.Remove((Action)_dictAllEvents[eventType], method);
        }

        public void RemoveEvent<T>(string eventType, Action<T> method)
        {
            if (!HasEvent(eventType))
                return;
            _dictAllEvents[eventType] = (Action<T>)Delegate.Remove((Action<T>)_dictAllEvents[eventType], method);
        }

        public void RemoveEvent<T, U>(string eventType, Action<T, U> method)
        {
            if (!HasEvent(eventType))
                return;
            _dictAllEvents[eventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)_dictAllEvents[eventType], method);
        }

        public void RemoveEvent<T, U, K>(string eventType, Action<T, U, K> method)
        {
            if (!HasEvent(eventType))
                return;
            _dictAllEvents[eventType] = (Action<T, U, K>)Delegate.Remove((Action<T, U, K>)_dictAllEvents[eventType], method);
        }
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
        public void DispathEvent(string eventType)
        {
            if (!HasEvent(eventType))
                return;
            Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
            Action method;
            for (int i = 0; i < allDelegate.Length; i++)
            {
                if (allDelegate[i].GetType() != typeof(Action))
                    continue;

                method = (Action)allDelegate[i];
                if (method != null)
                    method.Invoke();
            }
        }

        public void DispathEvent<T>(string eventType, T p1)
        {
            if (!HasEvent(eventType))
                return;
            Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
            Action<T> method;
            for (int i = 0; i < allDelegate.Length; i++)
            {
                if (allDelegate[i].GetType() != typeof(Action<T>))
                    continue;

                method = (Action<T>)allDelegate[i];
                if (method != null)
                    method.Invoke(p1);
            }
        }

        public void DispathEvent<T, U>(string eventType, T p1, U p2)
        {
            if (!HasEvent(eventType))
                return;
            Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
            Action<T, U> method;
            for (int i = 0; i < allDelegate.Length; i++)
            {
                if (allDelegate[i].GetType() != typeof(Action<T, U>))
                    continue;

                method = (Action<T, U>)allDelegate[i];
                if (method != null)
                    method.Invoke(p1, p2);
            }
        }

        public void DispathEvent<T, U, K>(string eventType, T p1, U p2, K p3)
        {
            if (!HasEvent(eventType))
                return;
            Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
            Action<T, U, K> method;
            for (int i = 0; i < allDelegate.Length; i++)
            {
                if (allDelegate[i].GetType() != typeof(Action<T, U, K>))
                    continue;

                method = (Action<T, U, K>)allDelegate[i];
                if (method != null)
                    method.Invoke(p1, p2, p3);
            }
        }
        #endregion
    }
}
