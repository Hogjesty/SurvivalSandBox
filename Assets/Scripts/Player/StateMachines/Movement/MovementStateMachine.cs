using Player.StateMachines.Movement.States.SubStates;
using UnityEngine;

namespace Player.StateMachines.Movement {
    public class MovementStateMachine : StateMachine {
        
        [SerializeField] private Transform mainCamera;
        [SerializeField] private Transform groundPoint;

        [SerializeField] private float speed;
        [SerializeField] private float sprintCoefficient;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float gravitySpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallingBorder;

        [HideInInspector] public float lastSpeed;
        [HideInInspector] public bool isShiftPressed;
        
        private CharacterController characterController;

        public Idle idleState { get; private set; }
        public Jogging joggingState { get; private set; }
        public Running runningState { get; private set; }
        public Jumping jumpingState { get; private set; }
        public Falling fallingState { get; private set; }
        public Sneaking sneakingState { get; private set; }
        public Crouching crouchingState { get; private set; }

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

        protected override BaseState GetInitialState() {
            return idleState;
        }

        public float GetRotationSpeed => rotationSpeed;
        public Transform GetMainCamera => mainCamera;
        public Transform GetGroundPoint => groundPoint;
        public CharacterController GetCharacterController => characterController;
        public float GetSpeed => speed;
        public float GetSprintCoefficient => sprintCoefficient;
        public float GetGravitySpeed => gravitySpeed;
        public float GetJumpForce => jumpForce;
        public float GetFallingBorder => fallingBorder;
        
        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
            string content = CurrentState != null ? CurrentState.name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }
    }
}