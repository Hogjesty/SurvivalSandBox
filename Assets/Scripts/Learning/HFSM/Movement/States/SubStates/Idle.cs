using UnityEngine;

namespace Learning.HFSM.Movement.States.SubStates {
    public class Idle: Grounded {
        public Idle(StateMachine stateMachine) : base("Idle", stateMachine) {
        }

        public override void Update() {
            base.Update();

            if (new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).magnitude > 0) {
                sm.ChangeState(sm.joggingState);
            }
        }
    }
}