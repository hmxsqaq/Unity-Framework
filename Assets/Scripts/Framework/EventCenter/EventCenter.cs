using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public interface IEventInfo{}
    
    public class EventInfo : IEventInfo
    {
        public Action Actions;
    }
    public class EventInfo<T> : IEventInfo
    {
        public Action<T> Actions;
    }
    
    public class EventCenter : Singleton<EventCenter>
    {
        private readonly Dictionary<EventType, IEventInfo> _eventDic = new();

        /// <summary>
        /// No Parameter Event Add
        /// </summary>
        /// <param name="type">EventType</param>
        /// <param name="action">Action with no Parameter</param>
        public void AddEventListener(EventType type,Action action)
        {
            if (!_eventDic.ContainsKey(type))
            {
                _eventDic.Add(type,new EventInfo());
            }
            ((EventInfo)_eventDic[type]).Actions += action;
        }

        /// <summary>
        /// Generics Parameter Event Add
        /// </summary>
        /// <param name="type">EventType</param>
        /// <param name="action">Action with 1 Generics Parameter</param>
        /// <typeparam name="T"></typeparam>
        public void AddEventListener<T>(EventType type,Action<T> action)
        {
            if (!_eventDic.ContainsKey(type))
            {
                _eventDic.Add(type,new EventInfo<T>());
            }
            ((EventInfo<T>)_eventDic[type]).Actions += action;
        }

        /// <summary>
        /// No Parameter Event Remove
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(EventType type,Action action)
        {
            if (_eventDic.ContainsKey(type))
            {
                ((EventInfo)_eventDic[type]).Actions -= action;
            }
            else
            {
                Debug.LogError($"EventCenter-Remove-Key:{type} Not Find");
            }
        }

        /// <summary>
        /// Generics Parameter Event Remove
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public void RemoveEventListener<T>(EventType type,Action<T> action)
        {
            if (_eventDic.ContainsKey(type))
            {
                ((EventInfo<T>)_eventDic[type]).Actions -= action;
            }
            else
            {
                Debug.LogError($"EventCenter-Remove-Key:{type} Not Find");
            }
        }

        /// <summary>
        /// Trigger Event with no Parameter
        /// </summary>
        /// <param name="type"></param>
        public void Trigger(EventType type)
        {
            if (_eventDic.ContainsKey(type))
            {
                if (((EventInfo)_eventDic[type]).Actions == null)
                {
                    Debug.LogError($"EventCenter-Trigger-Key:{type} Is Empty");
                    return;
                }
                ((EventInfo)_eventDic[type]).Actions();
            }
            else
            {
                Debug.LogError($"EventCenter-Trigger-Key:{type} Not Find");
            }
        }

        /// <summary>
        /// Trigger Event with Generics Parameter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <typeparam name="T"></typeparam>
        public void Trigger<T>(EventType type,T info)
        {
            if (_eventDic.ContainsKey(type))
            {
                if (((EventInfo<T>)_eventDic[type]).Actions == null)
                {
                    Debug.LogError($"EventCenter-Trigger-Key:{type} Is Empty");
                    return;
                }
                ((EventInfo<T>)_eventDic[type]).Actions(info);
            }
            else
            {
                Debug.LogError($"EventCenter-Trigger-Key:{type} Not Find");
            }
        }

        /// <summary>
        /// Clear the Dictionary
        /// </summary>
        public void Clear()
        {
            _eventDic.Clear();
        }
    }

}