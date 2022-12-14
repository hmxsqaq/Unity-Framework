using UnityEngine;
using Framework;
using EventType = Framework.EventType;

namespace Test.EventCenterTest
{
    public class Manager : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Instance.AddEventListener<object>(EventType.Test,Test2);
        }

        private void Test2(object info)
        {
            Debug.Log("Manager");
        }
    }
}