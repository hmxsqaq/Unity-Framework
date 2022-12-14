using Framework;
using UnityEngine;
using EventType = Framework.EventType;

namespace Test.EventCenterTest
{
    public class Player : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Instance.AddEventListener<object>(EventType.Test,Test1);
        }
        
        private void Test1(object info)
        {
            Debug.Log($"Player{info}");
        }
    }
}