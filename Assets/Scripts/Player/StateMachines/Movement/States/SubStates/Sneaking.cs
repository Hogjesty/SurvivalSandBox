using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Sneaking : Grounded {
        private Vector3 direction;
        private float currentSpeed;
        
        public Sneaking(StateMachine stateMachine) : base(stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_SNEAKING, true);
            currentSpeed = movementStateMachine.GetSpeed * 0.5f;
            movementStateMachine.GetCharacterController.height = 1.05f;
            movementStateMachine.GetCharacterController.center = new Vector3(0, 0.6f, 0);
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
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_SNEAKING, false);
            movementStateMachine.lastSpeed = currentSpeed;
            movementStateMachine.GetCharacterController.height = 1.6f;
            movementStateMachine.GetCharacterController.center = new Vector3(0, 0.85f, 0);
        }

        private void CheckForTransition() {
            if (direction.magnitude <= 0) {
                movementStateMachine.ChangeState(movementStateMachine.crouchingState);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                movementStateMachine.ChangeState(movementStateMachine.joggingState);
            }
        }
    }
}