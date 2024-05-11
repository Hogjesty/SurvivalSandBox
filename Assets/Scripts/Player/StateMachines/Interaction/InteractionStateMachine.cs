using Inventory;
using Player.StateMachines.Interaction.States.SubStates;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;

namespace Player.StateMachines.Interaction {
    public class InteractionStateMachine : StateMachine {
        
        public int IsPickingUpBool => Animator.StringToHash("IsPickingUp");
        
        public Idle idleState { get; private set; }
        public PickingUp pickingUpState { get; private set; }

        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform startRayPoint;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TextMeshProUGUI interactPopupText;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private PlayerInventory playerInventory;

        [HideInInspector] public GameObject currentObject;

        private void Awake() {
            idleState = new Idle(this);
            pickingUpState = new PickingUp(this);
        }
        
        protected override BaseState GetInitialState() {
            return idleState;
        }
        
        public void SetAnimBool(int animHashedName, bool value) {
            playerAnimator.SetBool(animHashedName, value);
        }

        public void DestroyCurrentObject() => Destroy(currentObject);

        public LayerMask GetLayerMask => layerMask;
        public Transform GetStartRayPoint => startRayPoint;
        public Camera GetMainCamera => mainCamera;
        public TextMeshProUGUI GetInteractPopupText => interactPopupText;
        public PlayerInventory GetPlayerInventory => playerInventory;
        
        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10f, 110f, 400f, 100f));
            string content = "Interaction: " + (CurrentState != null ? CurrentState.name : "(no current state)");
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }
    }
}