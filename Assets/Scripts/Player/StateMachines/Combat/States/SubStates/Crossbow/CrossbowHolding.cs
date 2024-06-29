using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates.Crossbow {
    public class CrossbowHolding : BaseState {
        private readonly CombatStateMachine combatStateMachine;

        private bool isCrossbowCharged;
        
        public CrossbowHolding(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_HOLDING, true);
        }

        public override void Update() {
            CheckForTransitions();
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_HOLDING, false);
        }

        private void CheckForTransitions() {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowUnequippingState);
            }

            if (Input.GetKey(KeyCode.Mouse0) && isCrossbowCharged) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowAttackingState);
                isCrossbowCharged = false;
            }

            if (Input.GetKey(KeyCode.R) && !isCrossbowCharged) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowReloadingState);
                isCrossbowCharged = true;
            }
        }
    }
}