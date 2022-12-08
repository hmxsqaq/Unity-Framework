using Framework;
using UnityEngine;

namespace Test.EventCenterTest
{
    public class Player : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Instance.AddEventListener<object>(EventName.Test,Test1);
        }
        
        private void Test1(object info)
        {
            Debug.Log($"Player{info}");
        }
    }
}