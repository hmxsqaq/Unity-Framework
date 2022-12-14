using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 对象池(单例)
    /// 用于管理所有Pool
    /// </summary>
    public class ObjectPool : Singleton<ObjectPool>
    {
        private GameObject _poolObject;
        private readonly Dictionary<string, PoolData> _poolDic = new();

        protected override void OnInstanceCreate()
        {
            _poolObject = new GameObject
            {
                name = "ObjectPool"
            };
        }

        /// <summary>
        /// Add Object to ObjectPool
        /// </summary>
        /// <param name="poolName"> PoolName </param>
        /// <param name="gameObject"> Obj being pushed </param>
        public void Push(string poolName,GameObject gameObject)
        {
            if (!_poolDic.ContainsKey(poolName))
                _poolDic.Add(poolName,new PoolData(gameObject,_poolObject));
            
            _poolDic[poolName].Push(gameObject);
        }

        /// <summary>
        /// Get Object from Pool or instantiate it(Sync)
        /// </summary>
        /// <param name="poolName"> PoolName </param>
        /// <param name="path"> LoadPath </param>
        /// <returns> Obj being obtained or instantiated </returns>
        public GameObject GetSync(string poolName,string path)
        {
            GameObject gameObject;
            if (_poolDic.ContainsKey(poolName) && _poolDic[poolName].ObjectList.Count > 0)
                gameObject = _poolDic[poolName].Get();
            else
            {
                gameObject = ResourcesManager.Instance.LoadSync<GameObject>(path);
                gameObject.name = poolName;
            }
            return gameObject;
        }

        /// <summary>
        /// Get Object from Pool or instantiate it(Async)
        /// </summary>
        /// <param name="poolName"> PoolName </param>
        /// <param name="path"> LoadPath </param>
        /// <param name="callback"> CallbackFunction </param>
        public void GetAsync(string poolName,string path,Action<GameObject> callback = null)
        {
            if (_poolDic.ContainsKey(poolName) && _poolDic[poolName].ObjectList.Count > 0)
            {
                GameObject obj = _poolDic[poolName].Get();
                callback?.Invoke(obj);
            }
            else
            {
                ResourcesManager.Instance.LoadAsync<GameObject>(path, (obj) =>
                {
                    obj.name = poolName;
                    callback?.Invoke(obj);
                });
            }
        }

        /// <summary>
        /// Clear the Dic and ParentObj
        /// </summary>
        public void Clear()
        {
            _poolDic.Clear();
            _poolObject = null;
        }
    }
}