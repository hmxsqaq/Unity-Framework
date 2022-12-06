using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    /// <summary>
    /// Sync and Async Load
    /// </summary>
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        public T LoadSync<T>(string name) where T : Object
        {
            T resource = Resources.Load<T>(name);
            if (resource is GameObject)
            {
                return Object.Instantiate(resource);
            }
            return resource;
        }

        public void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            MonoManager.Instance.StartCoroutine(AsyncLoad(name, callback));
        }

        private IEnumerator AsyncLoad<T>(string name, Action<T> callback) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(name);
            yield return request;
            if (request.asset is GameObject)
                callback(Object.Instantiate(request.asset) as T);
            else
                callback(request.asset as T);
        }
    }
}