using System.Collections;
using Framework;
using UnityEngine;

namespace Test.StateTest
{
    public class TestChaseStateTransition : ITransition<TestStateType>
    {
        private float _current;
        private float _time;

        public TestChaseStateTransition(float time)
        {
            _time = time;
        }
        public bool Transition(out TestStateType type)
        {
            _current += Time.deltaTime;
            if (_current >= _time)
            {
                _current = 0;
                type = TestStateType.Idle;
                return true;
            }
            type = TestStateType.Chase;
            return false;
        }
    }
}