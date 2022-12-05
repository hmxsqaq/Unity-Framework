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
        private Dictionary<string, IEventInfo> _eventDic = new();
        
        /// <summary>
        /// No Parameter Event Add
        /// </summary>
        /// <param name="name"> EventName </param>
        /// <param name="action"> Action with no Parameter </param>
        public void AddEventListener(string name,Action action)
        {
            if (!_eventDic.ContainsKey(name))
            {
                _eventDic.Add(name,new EventInfo());
            }
            ((EventInfo)_eventDic[name]).Actions += action;
        }

        /// <summary>
        /// Generics Parameter Event Add
        /// </summary>
        /// <param name="name"> EventName </param>
        /// <param name="action"> Action with 1 Generics Parameter </param>
        /// <typeparam name="T"> Generics Parameter(user defined) </typeparam>
        public void AddEventListener<T>(string name,Action<T> action)
        {
            if (!_eventDic.ContainsKey(name))
            {
                _eventDic.Add(name,new EventInfo<T>());
            }
            ((EventInfo<T>)_eventDic[name]).Actions += action;
        }

        /// <summary>
        /// No Parameter Event Remove
        /// </summary>
        /// <param name="name"> EventName </param>
        /// <param name="action"> Action with no Parameter </param>
        public void RemoveEventListener(string name,Action action)
        {
            if (_eventDic.ContainsKey(name))
            {
                ((EventInfo)_eventDic[name]).Actions -= action;
            }
            else
            {
                Debug.LogError($"EventCenter-Remove-Key:{name} Not Find");
            }
        }
        
        /// <summary>
        /// Generics Parameter Event Remove
        /// </summary>
        /// <param name="name"> EventName </param>
        /// <param name="action"> Action with 1 Generics Parameter </param>
        /// <typeparam name="T"> Generics Parameter(user defined) </typeparam>
        public void RemoveEventListener<T>(string name,Action<T> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                ((EventInfo<T>)_eventDic[name]).Actions -= action;
            }
            else
            {
                Debug.LogError($"EventCenter-Remove-Key:{name} Not Find");
            }
        }

        /// <summary>
        /// Trigger Event with no Parameter
        /// </summary>
        /// <param name="name"> EventName </param>
        public void Trigger(string name)
        {
            if (_eventDic.ContainsKey(name))
            {
                if (((EventInfo)_eventDic[name]).Actions == null)
                {
                    Debug.LogError($"EventCenter-Trigger-Key:{name} Is Empty");
                    return;
                }
                ((EventInfo)_eventDic[name]).Actions();
            }
            else
            {
                Debug.LogError($"EventCenter-Trigger-Key:{name} Not Find");
            }
        }
        
        /// <summary>
        /// Trigger Event with Generics Parameter
        /// </summary>
        /// <param name="name"> EventName </param>
        /// <param name="info"> Given Parameter </param>
        /// <typeparam name="T"> Generics Parameter(user defined) </typeparam>
        public void Trigger<T>(string name,T info)
        {
            if (_eventDic.ContainsKey(name))
            {
                if (((EventInfo<T>)_eventDic[name]).Actions == null)
                {
                    Debug.LogError($"EventCenter-Trigger-Key:{name} Is Empty");
                    return;
                }
                ((EventInfo<T>)_eventDic[name]).Actions(info);
            }
            else
            {
                Debug.LogError($"EventCenter-Trigger-Key:{name} Not Find");
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

    public static class EventName
    {
        public const string SceneStart = "SceneStart";
        public const string SceneLoading = "SceneLoading";
        public const string SceneEnd = "SceneEnd";
    }
}