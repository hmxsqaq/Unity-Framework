using System.Collections.Generic;
using Framework.Singleton;
using UnityEngine;

namespace Framework.ObjectPool
{
    /// <summary>
    /// 对象池(单例)
    /// 用于管理所有Pool
    /// </summary>
    public class ObjectPool : Singleton<ObjectPool>
    {
        private GameObject _poolObject;
        private Dictionary<string, Pool> _poolDic = new Dictionary<string, Pool>();

        // Add Object to ObjectPool
        public void Push(string poolName,GameObject gameObject)
        {
            if (_poolObject == null)
                _poolObject = new GameObject("ObjectPool");

            if (_poolDic.ContainsKey(poolName))
                _poolDic[poolName].Push(gameObject);
            else
                _poolDic.Add(poolName,new Pool(gameObject,_poolObject));
        }

        // Get Object from Object or instantiate it
        public GameObject Get(string poolName)
        {
            GameObject gameObject;
            if (_poolDic.ContainsKey(poolName) && _poolDic[poolName].ObjectList.Count > 0)
                gameObject = _poolDic[poolName].Get();
            else
            {
                gameObject = Object.Instantiate(Resources.Load<GameObject>(poolName));
                gameObject.name = poolName;
            }
            return gameObject;
        }

        // Clear the Dic and Obj
        public void Clear()
        {
            _poolDic.Clear();
            _poolObject = null;
        }
    }

    /// <summary>
    /// 单一池
    /// 用于管理某一类Object
    /// </summary>
    public class Pool
    {
        public GameObject ParentObject;
        public List<GameObject> ObjectList;

        public Pool(GameObject gameObject,GameObject poolObject)
        {
            // Init
            ParentObject = new GameObject(gameObject.name);
            ObjectList = new List<GameObject>();
            ParentObject.transform.parent = poolObject.transform;
            Push(gameObject);
        }

        public void Push(GameObject gameObject)
        {
            // View
            gameObject.SetActive(false);
            gameObject.transform.parent = ParentObject.transform;
            // Model
            ObjectList.Add(gameObject);
        }

        public GameObject Get()
        {
            GameObject gameObject = ObjectList[0];
            // View
            gameObject.SetActive(true);
            gameObject.transform.parent = null;
            // Model
            ObjectList.RemoveAt(0);
            return gameObject;
        }
    }
}