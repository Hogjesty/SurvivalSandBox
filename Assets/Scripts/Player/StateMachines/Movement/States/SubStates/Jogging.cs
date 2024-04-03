using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Jogging : AdaptiveGrounded  {
        private Vector3 direction;
        private float currentSpeed;

        public Jogging(StateMachine stateMachine) : base("Jogging", stateMachine) {
        }
        
        public override void Enter() {
            base.Enter();
            currentSpeed = movementStateMachine.GetSpeed;
        }

        public override void Update() {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            base.Update();
            CheckForTransition();

            movementStateMachine.Move(
                direction,
                currentSpeed,
                movementStateMachine.GetRotationSpeed,
                -15
            );
        }
        
        public override void Exit() {
            base.Exit();
            movementStateMachine.lastSpeed = currentSpeed;
        }

        private void CheckForTransition() {
            if (direction.magnitude <= 0) {
                movementStateMachine.ChangeState(movementStateMachine.idleState);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                movementStateMachine.isShiftPressed = true;
                movementStateMachine.ChangeState(movementStateMachine.runningState);
            }
        }
    }
}