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
        public T LoadSync<T>(string path) where T : Object
        {
            T resource = Resources.Load<T>(path);
            if (resource is GameObject)
            {
                return Object.Instantiate(resource);
            }
            return resource;
        }

        public void LoadAsync<T>(string path, Action<T> callback = null) where T : Object
        {
            MonoManager.Instance.StartCoroutine(AsyncLoad(path, callback));
        }

        private IEnumerator AsyncLoad<T>(string path, Action<T> callback = null) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;
            if (request.asset is GameObject)
                callback?.Invoke(Object.Instantiate(request.asset) as T);
            else
                callback?.Invoke(request.asset as T);
        }
    }
}