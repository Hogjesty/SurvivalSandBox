using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Running: AdaptiveGrounded  {
        
        private Vector3 direction;
        private float currentSpeed;
        
        public Running(StateMachine stateMachine) : base(stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_RUNNING, true);
            currentSpeed = movementStateMachine.GetSpeed * movementStateMachine.GetSprintCoefficient;
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
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_RUNNING, false);
            movementStateMachine.lastSpeed = currentSpeed;
        }

        private void CheckForTransition() {
            if (direction.magnitude <= 0) {
                movementStateMachine.ChangeState(movementStateMachine.idleState);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                movementStateMachine.isShiftPressed = false;
                movementStateMachine.ChangeState(movementStateMachine.joggingState);
            }
        }
    }
}