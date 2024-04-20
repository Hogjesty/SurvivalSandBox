using CombatEssentials;
using Player.AnimationsEvents;
using UnityEngine;

namespace Player.StateMachines.Combat.States.SubStates {
    public class Attacking : BaseState {
        protected readonly CombatStateMachine combatStateMachine;

        private bool isAnimationEnded;
        private float hitRadius;
        
        public Attacking(StateMachine stateMachine) : base("Attacking", stateMachine) {
            combatStateMachine = (CombatStateMachine) this.stateMachine;
            PlayerAnimationsEvents.endOfAttackingAnim += EndOfAttackingAnim;
            PlayerAnimationsEvents.hitPointAttackingAnim += HitPointAttackingAnim;
        }

        public override void Enter() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsAttackingBool, true);
            isAnimationEnded = false;

            hitRadius = 0.5f;
        }

        public override void Update() {
            if (isAnimationEnded) {
                combatStateMachine.ChangeState(combatStateMachine.holdingWeaponState);
            }
        }

        public override void Exit() {
            combatStateMachine.SetAnimBool(combatStateMachine.IsAttackingBool, false);
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

        ~Attacking() {
            PlayerAnimationsEvents.endOfAttackingAnim -= EndOfAttackingAnim;
            PlayerAnimationsEvents.hitPointAttackingAnim -= HitPointAttackingAnim;
        }
    }
}