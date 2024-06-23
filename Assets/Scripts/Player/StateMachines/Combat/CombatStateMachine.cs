using Player.StateMachines.Combat.States.SubStates;
using Player.StateMachines.Combat.States.SubStates.Crossbow;
using Player.StateMachines.Combat.States.SubStates.Sword;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.StateMachines.Combat {
    public class CombatStateMachine : StateMachine {

        public static readonly int IS_SWORD_EQUIPPING = Animator.StringToHash("IsSwordEquipping");
        public static readonly int IS_SWORD_HOLDING = Animator.StringToHash("IsSwordHolding");
        public static readonly int IS_SWORD_UNEQUIPPING = Animator.StringToHash("IsSwordUnequipping");
        public static readonly int IS_SWORD_ATTACKING = Animator.StringToHash("IsSwordAttacking");
        
        public static readonly int IS_CROSSBOW_EQUIPPING = Animator.StringToHash("IsCrossbowEquipping");
        public static readonly int IS_CROSSBOW_HOLDING = Animator.StringToHash("IsCrossbowHolding");
        public static readonly int IS_CROSSBOW_UNEQUIPPING = Animator.StringToHash("IsCrossbowUnequipping");
        public static readonly int IS_CROSSBOW_ATTACKING = Animator.StringToHash("IsCrossbowAttacking");
        public static readonly int IS_CROSSBOW_RELOADING = Animator.StringToHash("IsCrossbowReloading");
        
        public Idle idleState { get; private set; }
        public SwordEquipping swordEquippingState { get; private set; }
        public SwordHolding swordHoldingState { get; private set; }
        public SwordUnequipping swordUnequippingState { get; private set; }
        public SwordAttacking swordAttackingState { get; private set; }
        
        public CrossbowEquipping crossbowEquippingState { get; private set; }
        public CrossbowHolding crossbowHoldingState { get; private set; }
        public CrossbowUnequipping crossbowUnequippingState { get; private set; }
        public CrossbowAttacking crossbowAttackingState { get; private set; }
        public CrossbowReloading crossbowReloadingState { get; private set; }

        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Transform hitPoint;
        [SerializeField] private LayerMask enemyLayers;
        
        [SerializeField] private GameObject swordInHand;
        [SerializeField] private GameObject swordInBelt;
        [SerializeField] private GameObject crossbowInHand;

        private void Awake() {
            idleState = new Idle(this);
            swordEquippingState = new SwordEquipping(this);
            swordHoldingState = new SwordHolding(this);
            swordUnequippingState = new SwordUnequipping(this);
            swordAttackingState = new SwordAttacking(this);
            crossbowEquippingState = new CrossbowEquipping(this);
            crossbowHoldingState = new CrossbowHolding(this);
            crossbowUnequippingState = new CrossbowUnequipping(this);
            crossbowAttackingState = new CrossbowAttacking(this);
            crossbowReloadingState = new CrossbowReloading(this);
        }

        protected override BaseState GetInitialState() {
            return idleState;
        }

        public void SetAnimBool(int animHashedName, bool value) {
            playerAnimator.SetBool(animHashedName, value);
        }

        public Transform GetHitPoint => hitPoint;
        public LayerMask GetEnemyLayers => enemyLayers;
        public GameObject GetSwordInHand => swordInHand;
        public GameObject GetSwordInBelt => swordInBelt;
        public GameObject GetCrossbowInHand => crossbowInHand;

        // private void OnGUI() {
        //     GUILayout.BeginArea(new Rect(10f, 60f, 700f, 100f));
        //     string content = "Combat: " + (CurrentState != null ? CurrentState.name : "(no current state)");
        //     GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        //     GUILayout.EndArea();
        // }
    }
}