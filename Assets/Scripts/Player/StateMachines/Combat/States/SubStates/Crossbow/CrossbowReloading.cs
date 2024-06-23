using Player.AnimationsEvents;

namespace Player.StateMachines.Combat.States.SubStates.Crossbow {
    public class CrossbowReloading : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        private bool isAnimationEnded;
        
        public CrossbowReloading(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfCrossbowReloadingAnim += EndOfReloadingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_RELOADING, true);
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
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_RELOADING, false);
        }
        
        private void EndOfReloadingAnim() {
            isAnimationEnded = true;
        }

        ~CrossbowReloading() {
            PlayerAnimationsEvents.endOfCrossbowReloadingAnim -= EndOfReloadingAnim;
        }
    }
}