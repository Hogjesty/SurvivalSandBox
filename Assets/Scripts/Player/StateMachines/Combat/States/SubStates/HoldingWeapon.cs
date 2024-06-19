using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates {
    public class HoldingWeapon : BaseState {
        protected readonly CombatStateMachine combatStateMachine;
        
        public HoldingWeapon(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsHoldingWeaponBool, true);
        }

        public override void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                combatStateMachine.ChangeState(combatStateMachine.unequippingWeaponState);
            }

            if (Input.GetKey(KeyCode.Mouse0)) {
                combatStateMachine.ChangeState(combatStateMachine.attackingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsHoldingWeaponBool, false);
        }
    }
}