using UnityEngine;

namespace Player.StateMachines.Movement.States {
    public class AdaptiveGrounded : Grounded {
        
        protected AdaptiveGrounded(StateMachine stateMachine) : base(stateMachine) {
        }
        
        public override void Enter() {
        }

        public override void Update() {
            base.Update();
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                movementStateMachine.ChangeState(movementStateMachine.jumpingState);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                movementStateMachine.ChangeState(movementStateMachine.crouchingState);
            }
        }

        public override void Exit() {
        }
        
    }
}