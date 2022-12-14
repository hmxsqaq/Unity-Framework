using Test.StateTest;
using UnityEngine;

namespace Framework
{
    public class FsmFactory
    {
        public static FsmSystem<TestStateType> CreateTestFsm(float idleTime,float chaseTime)
        {
            var fsm = new FsmSystem<TestStateType>();

            var idleTransition = new TestIdleStateTransition(idleTime);
            var chaseTransition = new TestChaseStateTransition(chaseTime);

            var idleState = new TestIdleState(idleTransition);
            var chaseState = new TestChaseState(chaseTransition);

            fsm.AddState(idleState);
            fsm.AddState(chaseState);

            fsm.StartState(TestStateType.Idle);
            return fsm;
        }
    }
}