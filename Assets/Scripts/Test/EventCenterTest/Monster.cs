using System;
using Framework;
using UnityEngine;

namespace Test.EventCenterTest
{
    public class Monster : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Instance.Trigger<object>("1",this);
        }
    }
}