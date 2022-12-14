using Framework;
using UnityEngine;

namespace Test.StateTest
{
    public class TestChaseState : StateBase<TestStateType>
    {
        public TestChaseState(ITransition<TestStateType> transition) : base(TestStateType.Chase, transition)
        {
        }

        public override void OnUpdate()
        {
            Debug.Log("Chase Update");
        }
    }
}