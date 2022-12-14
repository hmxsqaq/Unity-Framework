using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 单一池
    /// 用于管理某一类Object
    /// </summary>
    public class PoolData
    {
        private readonly GameObject _parentObject;
        public readonly List<GameObject> ObjectList;
        
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="gameObject"> the first pushed Obj </param>
        /// <param name="poolObject"> empty parent Obj to all pool </param>
        public PoolData(GameObject gameObject,GameObject poolObject)
        {
            _parentObject = new GameObject(gameObject.name);
            ObjectList = new List<GameObject>();
            _parentObject.transform.parent = poolObject.transform;
        }

        /// <summary>
        /// Disable the given Obj and Add it to List
        /// </summary>
        /// <param name="gameObject"> Obj </param>
        public void Push(GameObject gameObject)
        {
            // View
            gameObject.SetActive(false);
            gameObject.transform.parent = _parentObject.transform;
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