using Player.AnimationsEvents;

namespace Player.StateMachines.Combat.States.SubStates.Crossbow {
    public class CrossbowEquipping : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        private bool isAnimationEnded;
        
        public CrossbowEquipping(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfCrossbowEquippingAnim += EndOfCrossbowEquippingAnim;
            PlayerAnimationsEvents.pointOfCrossbowEquippingAnim += PointOfCrossbowEquippingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_EQUIPPING, true);
            isAnimationEnded = false;
        }

        public override void Update() {
            if (isAnimationEnded) {
                combatStateMachine.ChangeState(combatStateMachine.crossbowHoldingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_EQUIPPING, false);
        }
        
        private void EndOfCrossbowEquippingAnim() {
            isAnimationEnded = true;
        }

        private void PointOfCrossbowEquippingAnim() {
            combatStateMachine.GetCrossbowInHand.SetActive(true);
        }
        
        ~CrossbowEquipping() {
            PlayerAnimationsEvents.endOfCrossbowEquippingAnim -= EndOfCrossbowEquippingAnim;
            PlayerAnimationsEvents.pointOfCrossbowEquippingAnim -= PointOfCrossbowEquippingAnim;
        }
    }
}