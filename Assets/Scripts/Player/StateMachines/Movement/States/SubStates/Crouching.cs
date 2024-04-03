using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Crouching : Grounded {
        private Vector3 direction;
        
        public Crouching(StateMachine stateMachine) : base("Crouching", stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            movementStateMachine.GetCharacterController.height = 1.05f;
            movementStateMachine.GetCharacterController.center = new Vector3(0, 0.6f, 0);
        }

        public override void Update() {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            base.Update();
            CheckForTransition();
        }

        public override void Exit() {
            base.Exit();
            movementStateMachine.GetCharacterController.height = 1.6f;
            movementStateMachine.GetCharacterController.center = new Vector3(0, 0.85f, 0);
        }

        private void CheckForTransition() {
            if (direction.magnitude > 0) {
                movementStateMachine.ChangeState(movementStateMachine.sneakingState);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                movementStateMachine.ChangeState(movementStateMachine.idleState);
            }
        }
    }
}