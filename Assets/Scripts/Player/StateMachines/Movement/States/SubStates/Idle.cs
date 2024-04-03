using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Idle: AdaptiveGrounded  {
        public Idle(StateMachine stateMachine) : base("Idle", stateMachine) {
        }

        public override void Enter() {
            base.Enter();

            movementStateMachine.isShiftPressed = false;
        }

        public override void Update() {
            base.Update();

            if (new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).magnitude > 0) {
                movementStateMachine.ChangeState(movementStateMachine.joggingState);
            }
        }

        public override void Exit() {
            base.Exit();
            movementStateMachine.lastSpeed = 0;
        }
    }
}