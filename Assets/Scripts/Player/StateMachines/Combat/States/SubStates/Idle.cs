using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates {
    public class Idle : BaseState {
        protected readonly CombatStateMachine movementStateMachine;
        
        public Idle(StateMachine stateMachine) : base(stateMachine) {
            movementStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            
        }

        public override void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                movementStateMachine.ChangeState(movementStateMachine.equippingWeaponState);
            }
        }

        public override void Exit() {
            
        }
    }
}