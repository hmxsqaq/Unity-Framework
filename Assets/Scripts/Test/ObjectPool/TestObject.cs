using System.Collections;
using UnityEngine;
using Framework;
using Framework.ObjectPool;

namespace Test
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