﻿using UnityEngine;

namespace Framework.Singleton
{
    public class SingletonMono<T> : MonoBehaviour where T:SingletonMono<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<T>();
                    _instance.OnInstanceCreate();
                }
                return _instance;
            }
        }
        
        protected virtual void OnInstanceCreate(){}
    }
}