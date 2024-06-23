using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates.Crossbow {
    public class CrossbowHolding : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        public CrossbowHolding(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_HOLDING, true);
        }

        public override void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowUnequippingState);
            }

            if (Input.GetKey(KeyCode.Mouse0)) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowAttackingState);
            }

            if (Input.GetKey(KeyCode.R)) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowReloadingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_HOLDING, false);
        }
    }
}