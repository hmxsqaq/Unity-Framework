using UnityEngine;

namespace Test.EventCenterTest
{
    public class Player : MonoBehaviour
    {
        private void Start()
        {
            Framework.EventCenter.EventCenter.Instance.AddEventListener<object>("1",Test1);
        }
        
        private void Test1(object info)
        {
            Debug.Log($"Player{info}");
        }
    }
}