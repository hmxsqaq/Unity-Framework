using UnityEngine;
using Framework;

namespace Test.EventCenterTest
{
    public class Manager : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Instance.AddEventListener<object>("1",Test2);
        }

        private void Test2(object info)
        {
            Debug.Log("Manager");
        }
    }
}