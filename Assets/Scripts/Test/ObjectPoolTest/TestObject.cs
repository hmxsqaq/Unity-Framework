using System.Collections;
using Framework;
using UnityEngine;

namespace Test.ObjectPoolTest
{

    public class TestObject : MonoBehaviour
    {
        public float fadetime;

        private void OnEnable()
        {
            StartCoroutine(Fade(fadetime));
        }

        IEnumerator Fade(float time)
        {
            yield return new WaitForSeconds(time);
            ObjectPool.Instance.Push(gameObject.name,gameObject);
        }
    }
}