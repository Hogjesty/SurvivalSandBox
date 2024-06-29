using Player.StateMachines.Combat.States.SubStates.Crossbow;
using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates {
    public class Idle : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        public Idle(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            
        }

        public override void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                combatStateMachine.ChangeState(combatStateMachine.swordEquippingState);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowEquippingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            
        }
    }
}