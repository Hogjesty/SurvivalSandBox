using Player.AnimationsEvents;

namespace Player.StateMachines.Combat.States.SubStates.Sword {
    public class SwordUnequipping : BaseState {
        private readonly CombatStateMachine combatStateMachine;

        private bool isAnimationEnded;
        
        public SwordUnequipping(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfSwordUnequippingAnim += EndOfWeaponUnequippingAnim;
            PlayerAnimationsEvents.pointOfSwordUnequippingAnim += PointOfWeaponUnequippingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_UNEQUIPPING, true);
            isAnimationEnded = false;
        }

        public override void Update() {
            if (isAnimationEnded) {
                combatStateMachine.ChangeState(combatStateMachine.idleState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_UNEQUIPPING, false);
        }

        private void EndOfWeaponUnequippingAnim() {
            isAnimationEnded = true;
        }

        private void PointOfWeaponUnequippingAnim() {
            combatStateMachine.GetSwordInHand.SetActive(false);
            combatStateMachine.GetSwordInBelt.SetActive(true);
        }
        
        ~SwordUnequipping() {
            PlayerAnimationsEvents.endOfSwordEquippingAnim -= EndOfWeaponUnequippingAnim;
            PlayerAnimationsEvents.pointOfSwordUnequippingAnim -= PointOfWeaponUnequippingAnim;
        }
    }
}