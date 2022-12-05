using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace Test.MonoManagerTest
{
    public class NoMonoTest
    {
        public void Update()
        {
            Debug.Log("test");
        }

        public IEnumerator CoroutineTest()
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("Coroutine");
        }
    }

    public class MonoTest : MonoBehaviour
    {
        private void Start()
        {
            MonoManager.Instance.StartCoroutine(new NoMonoTest().CoroutineTest());
        }
    }
}