using UnityEngine;

namespace Learning.HFSM.Movement.States {
    public class Grounded : BaseState {
        protected MovementSM sm;

        protected Grounded(string name, StateMachine stateMachine) : base(name, stateMachine) {
            sm = (MovementSM) this.stateMachine;
        }

        public override void Update() {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space)) {
                sm.ChangeState(sm.jumpingState);
            }
        }
    }
}