using Player.AnimationsEvents;
using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates.Sword {
    public class SwordEquipping : BaseState {
        private readonly CombatStateMachine combatStateMachine;

        private bool isAnimationEnded;
        
        public SwordEquipping(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfSwordEquippingAnim += EndOfWeaponEquippingAnim;
            PlayerAnimationsEvents.pointOfSwordEquippingAnim += PointOfWeaponEquippingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_EQUIPPING, true);
            isAnimationEnded = false;
        }

        public override void Update() {
            if (isAnimationEnded) {
                combatStateMachine.ChangeState(combatStateMachine.swordHoldingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_EQUIPPING, false);
        }

        private void EndOfWeaponEquippingAnim() {
            isAnimationEnded = true;
        }

        private void PointOfWeaponEquippingAnim() {
            combatStateMachine.GetSwordInHand.SetActive(true);
            combatStateMachine.GetSwordInBelt.SetActive(false);
        }

        ~SwordEquipping() {
            PlayerAnimationsEvents.endOfSwordEquippingAnim -= EndOfWeaponEquippingAnim;
            PlayerAnimationsEvents.pointOfSwordEquippingAnim -= PointOfWeaponEquippingAnim;
        }
    }
}