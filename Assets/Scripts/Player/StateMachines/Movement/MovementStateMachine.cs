using Player.StateMachines.Movement.States.SubStates;
using UnityEngine;

namespace Player.StateMachines.Movement {
    public class MovementStateMachine : StateMachine {
        public static readonly int IS_IDLE = Animator.StringToHash("IsIdle");
        public static readonly int IS_JOGGING = Animator.StringToHash("IsJogging");
        public static readonly int IS_RUNNING = Animator.StringToHash("IsRunning");
        public static readonly int IS_JUMPING = Animator.StringToHash("IsJumping");
        public static readonly int IS_FALLING = Animator.StringToHash("IsFalling");
        public static readonly int IS_SNEAKING = Animator.StringToHash("IsSneaking");
        public static readonly int IS_CROUCHING = Animator.StringToHash("IsCrouching");
        
        public Idle idleState { get; private set; }
        public Jogging joggingState { get; private set; }
        public Running runningState { get; private set; }
        public Jumping jumpingState { get; private set; }
        public Falling fallingState { get; private set; }
        public Sneaking sneakingState { get; private set; }
        public Crouching crouchingState { get; private set; }
        
        [SerializeField] private Transform mainCamera;
        [SerializeField] private Transform groundPoint;
        [SerializeField] private Transform sphereCastPointGround;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private StateMachineManager stateMachineManager;

        [SerializeField] private float speed;
        [SerializeField] private float sprintCoefficient;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float gravitySpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallingBorder;
        [SerializeField] private float slippingSphereCastRadius;
        [SerializeField] private float onGroundSphereRadius;
        [SerializeField] private float slippingSpeed;

        [HideInInspector] public float lastSpeed;
        [HideInInspector] public bool isShiftPressed;

        private CharacterController characterController;

        private void Awake() {
            idleState = new Idle(this);
            joggingState = new Jogging(this);
            runningState = new Running(this);
            jumpingState = new Jumping(this);
            fallingState = new Falling(this);
            sneakingState = new Sneaking(this);
            crouchingState = new Crouching(this);
            
            characterController = GetComponent<CharacterController>();
        }

        public void Move(Vector3 direction, float speed, float rotationSpeed, float gravity) {
            if (direction.magnitude > 0) {
                float rotationY = mainCamera.rotation.eulerAngles.y + Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.Euler(0, rotationY, 0),
                    rotationSpeed
                );
            }
            
            Vector3 globalMovingDirection = transform.TransformDirection(0, gravity, direction.normalized.magnitude * speed);
            characterController.Move(globalMovingDirection * Time.deltaTime);
        }

        public void Rotate(float rotationSpeed) {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y, 0),
                rotationSpeed
            );
        }

        protected override BaseState GetInitialState() {
            return idleState;
        }

        public float GetRotationSpeed => rotationSpeed;
        public Transform GetGroundPoint => groundPoint;
        public Transform GetSphereCastPointGround => sphereCastPointGround;
        public Animator GetPlayerAnimator => playerAnimator;
        public CharacterController GetCharacterController => characterController;
        public float GetSpeed => speed;
        public float GetSprintCoefficient => sprintCoefficient;
        public float GetGravitySpeed => gravitySpeed;
        public float GetJumpForce => jumpForce;
        public float GetFallingBorder => fallingBorder;
        public float GetSlippingSphereCastRadius => slippingSphereCastRadius;
        public float GetOnGroundSphereRadius => onGroundSphereRadius;
        public float GetSlippingSpeed => slippingSpeed;
        public StateMachineManager GetStateMachineManager => stateMachineManager;
        
        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10f, 10f, 400f, 100f));
            string content = "Movement: " + (CurrentState != null ? CurrentState.name : "(no current state)");
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(GetGroundPoint.position, GetOnGroundSphereRadius);
        }
    }
}