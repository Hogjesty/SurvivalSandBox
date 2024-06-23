using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates.Sword {
    public class SwordHolding : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        public SwordHolding(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_HOLDING, true);
        }

        public override void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                combatStateMachine.ChangeState(combatStateMachine.swordUnequippingState);
            }

            if (Input.GetKey(KeyCode.Mouse0)) {
                combatStateMachine.ChangeState(combatStateMachine.swordAttackingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_HOLDING, false);
        }
    }
}