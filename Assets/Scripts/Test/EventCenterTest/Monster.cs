using System;
using UnityEngine;

namespace Test.EventCenterTest
{
    public class Monster : MonoBehaviour
    {
        private void Start()
        {
            Framework.EventCenter.EventCenter.Instance.Trigger<object>("1",this);
        }
    }
}