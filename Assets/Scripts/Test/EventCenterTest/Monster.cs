using System;
using Framework;
using UnityEngine;
using EventType = Framework.EventType;

namespace Test.EventCenterTest
{
    public class Monster : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Instance.Trigger<object>(EventType.Test,this);
        }
    }
}