using Framework;
using UnityEngine;

namespace Test.ObjectPoolTest
{
    public class ObjectGenerator : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ObjectPool.Instance.Get("Prefabs/ObjectPool/Cube");
            }
            if (Input.GetMouseButtonDown(1))
            {
                ObjectPool.Instance.Get("Prefabs/ObjectPool/Sphere");
            }
        }
    }
}