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
                ObjectPool.Instance.GetAsync("Cube","Prefabs/ObjectPool/Cube", (o =>
                {
                    Debug.Log("AsyncGenerate");
                }));
            }
            if (Input.GetMouseButtonDown(1))
            {
                ObjectPool.Instance.GetSync("Sphere","Prefabs/ObjectPool/Sphere");
            }
        }
    }
}