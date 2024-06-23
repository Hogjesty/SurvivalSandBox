using CombatEssentials;
using Player.AnimationsEvents;
using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates.Sword {
    public class SwordAttacking : BaseState {
        private readonly CombatStateMachine combatStateMachine;

        private bool isAnimationEnded;
        private float hitRadius;
        
        public SwordAttacking(StateMachine stateMachine) : base(stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfSwordAttackingAnim += EndOfAttackingAnim;
            PlayerAnimationsEvents.hitPointSwordAttackingAnim += HitPointAttackingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_ATTACKING, true);
            isAnimationEnded = false;

            hitRadius = 0.5f;
        }

        public override void Update() {
            if (isAnimationEnded) {
                combatStateMachine.ChangeState(combatStateMachine.swordHoldingState);
            }
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(CombatStateMachine.IS_SWORD_ATTACKING, false);
        }
        
        private void EndOfAttackingAnim() {
            isAnimationEnded = true;
        }

        private void HitPointAttackingAnim() {
            Collider[] enemies = Physics.OverlapSphere(combatStateMachine.GetHitPoint.position, hitRadius, combatStateMachine.GetEnemyLayers);

            foreach(Collider enemy in enemies) {
                enemy.GetComponent<CombatAttributes>()?.TakeDamage(Random.Range(10, 20));
            }
        }

        ~SwordAttacking() {
            PlayerAnimationsEvents.endOfSwordAttackingAnim -= EndOfAttackingAnim;
            PlayerAnimationsEvents.hitPointSwordAttackingAnim -= HitPointAttackingAnim;
        }
    }
}