using Player.AnimationsEvents;
using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates {
    public class EquippingWeapon : BaseState {
        protected readonly CombatStateMachine combatStateMachine;

        private bool isAnimationEnded;
        
        public EquippingWeapon(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfWeaponEquippingAnim += EndOfWeaponEquippingAnim;
            PlayerAnimationsEvents.pointOfWeaponEquippingAnim += PointOfWeaponEquippingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsEquippingWeaponBool, true);
            isAnimationEnded = false;
        }

        public override void Update() {
            if (isAnimationEnded) {
                combatStateMachine.ChangeState(combatStateMachine.holdingWeaponState);
            }
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsEquippingWeaponBool, false);
        }

        private void EndOfWeaponEquippingAnim() {
            isAnimationEnded = true;
        }

        private void PointOfWeaponEquippingAnim() {
            combatStateMachine.GetWeaponInHand.SetActive(true);
            combatStateMachine.GetWeaponInBelt.SetActive(false);
        }

        ~EquippingWeapon() {
            PlayerAnimationsEvents.endOfWeaponEquippingAnim -= EndOfWeaponEquippingAnim;
            PlayerAnimationsEvents.pointOfWeaponEquippingAnim -= PointOfWeaponEquippingAnim;
        }
    }
}