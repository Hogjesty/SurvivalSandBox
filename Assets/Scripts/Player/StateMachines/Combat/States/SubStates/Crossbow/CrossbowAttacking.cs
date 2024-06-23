using Player.AnimationsEvents;

namespace Player.StateMachines.Combat.States.SubStates.Crossbow {
    public class CrossbowAttacking : BaseState {
        private readonly CombatStateMachine combatStateMachine;
        
        private bool isAnimationEnded;
        
        public CrossbowAttacking(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfCrossbowAttackingAnim += EndOfAttackingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_ATTACKING, true);
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
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_CROSSBOW_ATTACKING, false);
        }
        
        private void EndOfAttackingAnim() {
            isAnimationEnded = true;
        }

        ~CrossbowAttacking() {
            PlayerAnimationsEvents.endOfCrossbowAttackingAnim -= EndOfAttackingAnim;
        }
    }
}