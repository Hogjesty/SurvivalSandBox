using Player.AnimationsEvents;

namespace Player.StateMachines.Combat.States.SubStates.Crossbow {
    public class CrossbowUnequipping : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        private bool isAnimationEnded;
        
        public CrossbowUnequipping(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfCrossbowUnequippingAnim += EndOfCrossbowUnequippingAnim;
            PlayerAnimationsEvents.pointOfCrossbowUnequippingAnim += PointOfCrossbowUnequippingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_UNEQUIPPING, true);
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
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_UNEQUIPPING, false);
        }
        
        private void EndOfCrossbowUnequippingAnim() {
            isAnimationEnded = true;
        }

        private void PointOfCrossbowUnequippingAnim() {
            combatStateMachine.GetCrossbowInHand.SetActive(false);
        }
        
        ~CrossbowUnequipping() {
            PlayerAnimationsEvents.endOfCrossbowUnequippingAnim -= EndOfCrossbowUnequippingAnim;
            PlayerAnimationsEvents.pointOfCrossbowUnequippingAnim -= PointOfCrossbowUnequippingAnim;
        }
    }
}