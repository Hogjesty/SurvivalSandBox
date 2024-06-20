using System;
using Player.StateMachines.Combat.States.SubStates;
using UnityEngine;

namespace Player.StateMachines.Combat {
    public class CombatStateMachine : StateMachine {

        public int IsEquippingWeaponBool => Animator.StringToHash("IsEquippingWeapon");
        public int IsUnequippingWeaponBool => Animator.StringToHash("IsUnequippingWeapon");
        public int IsHoldingWeaponBool => Animator.StringToHash("IsHoldingWeapon");
        public int IsAttackingBool => Animator.StringToHash("IsAttacking");
        
        public Idle idleState { get; private set; }
        public EquippingWeapon equippingWeaponState { get; private set; }
        public HoldingWeapon holdingWeaponState { get; private set; }
        public UnequippingWeapon unequippingWeaponState { get; private set; }
        public Attacking attackingState { get; private set; }

        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Transform hitPoint;
        [SerializeField] private LayerMask enemyLayers;
        
        [SerializeField] private GameObject weaponInHand;
        [SerializeField] private GameObject weaponInBelt;

        private void Awake() {
            idleState = new Idle(this);
            equippingWeaponState = new EquippingWeapon(this);
            holdingWeaponState = new HoldingWeapon(this);
            unequippingWeaponState = new UnequippingWeapon(this);
            attackingState = new Attacking(this);
        }

        protected override BaseState GetInitialState() {
            return idleState;
        }

        public void SetAnimBool(int animHashedName, bool value) {
            playerAnimator.SetBool(animHashedName, value);
        }

        public Transform GetHitPoint => hitPoint;
        public LayerMask GetEnemyLayers => enemyLayers;
        public GameObject GetWeaponInHand => weaponInHand;
        public GameObject GetWeaponInBelt => weaponInBelt;

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10f, 60f, 700f, 100f));
            string content = "Combat: " + (CurrentState != null ? CurrentState.name : "(no current state)");
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }
    }
}