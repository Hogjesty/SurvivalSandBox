using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates {
    public class Idle : BaseState {
        private readonly CombatStateMachine movementStateMachine;
        
        public Idle(StateMachine stateMachine) : base(stateMachine) {
            movementStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            
        }

        public override void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                movementStateMachine.ChangeState(movementStateMachine.swordEquippingState);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                movementStateMachine.ChangeState(movementStateMachine.crossbowEquippingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            
        }
    }
}