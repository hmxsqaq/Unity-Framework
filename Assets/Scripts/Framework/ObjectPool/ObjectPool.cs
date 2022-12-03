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

        /// <summary>
        /// Add Object to ObjectPool
        /// </summary>
        /// <param name="poolName"> PoolName </param>
        /// <param name="gameObject"> Obj being pushed </param>
        public void Push(string poolName,GameObject gameObject)
        {
            if (_poolObject == null)
                _poolObject = new GameObject("ObjectPool");

            if (!_poolDic.ContainsKey(poolName))
                _poolDic.Add(poolName,new Pool(gameObject,_poolObject));
            
            _poolDic[poolName].Push(gameObject);
        }

        /// <summary>
        /// Get Object from Pool or instantiate it
        /// </summary>
        /// <param name="poolName"> PoolName </param>
        /// <returns> Obj being obtained or instantiated </returns>
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

        /// <summary>
        /// Clear the Dic and ParentObj
        /// </summary>
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
        
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="gameObject"> the first pushed Obj </param>
        /// <param name="poolObject"> empty parent Obj to all pool </param>
        public Pool(GameObject gameObject,GameObject poolObject)
        {
            ParentObject = new GameObject(gameObject.name);
            ObjectList = new List<GameObject>();
            ParentObject.transform.parent = poolObject.transform;
        }

        /// <summary>
        /// Disable the given Obj and Add it to List
        /// </summary>
        /// <param name="gameObject"> Obj </param>
        public void Push(GameObject gameObject)
        {
            // View
            gameObject.SetActive(false);
            gameObject.transform.parent = ParentObject.transform;
            // Model
            ObjectList.Add(gameObject);
        }

        /// <summary>
        /// Enable the first Obj in List and Return it
        /// </summary>
        /// <returns> the first Obj in List </returns>
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