using System;
using Framework;
using UnityEngine;

namespace Test.StateTest
{
    public class Test : MonoBehaviour
    {
        private FsmSystem<TestStateType> _fsmSystem;
        private void Awake()
        {
            _fsmSystem = FsmFactory.CreateTestFsm(2f, 1f);
        }

        private void Update()
        {
            _fsmSystem.Update();
        }
    }
}