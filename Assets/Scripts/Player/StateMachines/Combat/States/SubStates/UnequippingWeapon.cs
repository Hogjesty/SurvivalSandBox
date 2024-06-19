using Player.AnimationsEvents;

namespace Player.StateMachines.Combat.States.SubStates {
    public class UnequippingWeapon : BaseState {
        private readonly CombatStateMachine combatStateMachine;

        private bool isAnimationEnded;
        
        public UnequippingWeapon(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfWeaponUnequippingAnim += EndOfWeaponUnequippingAnim;
            PlayerAnimationsEvents.pointOfWeaponUnequippingAnim += PointOfWeaponUnequippingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsUnequippingWeaponBool, true);
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
            combatStateMachine.SetAnimBool(combatStateMachine.IsUnequippingWeaponBool, false);
        }

        private void EndOfWeaponUnequippingAnim() {
            isAnimationEnded = true;
        }

        private void PointOfWeaponUnequippingAnim() {
            combatStateMachine.GetWeaponInHand.SetActive(false);
            combatStateMachine.GetWeaponInBelt.SetActive(true);
        }
        
        ~UnequippingWeapon() {
            PlayerAnimationsEvents.endOfWeaponEquippingAnim -= EndOfWeaponUnequippingAnim;
            PlayerAnimationsEvents.pointOfWeaponUnequippingAnim -= PointOfWeaponUnequippingAnim;
        }
    }
}