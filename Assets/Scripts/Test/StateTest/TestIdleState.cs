using Framework;
using UnityEngine;

namespace Test.StateTest
{
    public class TestIdleState : StateBase<TestStateType>
    {
        public TestIdleState(ITransition<TestStateType> transition) : base(TestStateType.Idle, transition)
        {
        }

        public override void OnUpdate()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Idle Enter");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Idle Exit");
        }
    }
}