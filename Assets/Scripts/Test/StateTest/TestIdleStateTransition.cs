using System.Collections;
using Framework;
using UnityEngine;

namespace Test.StateTest
{
    public class TestIdleStateTransition : ITransition<TestStateType>
    {
        private float _time;
        private float _current;
        
        public TestIdleStateTransition(float time)
        {
            _time = time;
        }
        public bool Transition(out TestStateType type)
        {
            _current += Time.deltaTime;
            if (_current >= _time)
            {
                _current = 0;
                type = TestStateType.Chase;
                return true;
            }
            type = TestStateType.Idle;
            return false;
        }
    }
}