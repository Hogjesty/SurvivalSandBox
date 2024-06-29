using Effects;
using Player.AnimationsEvents;
using UnityEngine;

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
            
            Vector3 cameraForward = combatStateMachine.GetMainCamera.forward;
            bool raycast = Physics.Raycast(combatStateMachine.GetCrossbowInHand.transform.position,
                cameraForward, out RaycastHit hit);
            
            ArrowTrailMoveEffect arrowTrailMoveEffect = combatStateMachine.InstantiateGameObject(combatStateMachine.GetArrowTrail)
                .GetComponent<ArrowTrailMoveEffect>();
            arrowTrailMoveEffect.gameObject.transform.position = combatStateMachine.GetCrossbowInHand.transform.position;
            arrowTrailMoveEffect.Destination = raycast ? hit.point : combatStateMachine.GetMainCamera.position + cameraForward * 100;
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