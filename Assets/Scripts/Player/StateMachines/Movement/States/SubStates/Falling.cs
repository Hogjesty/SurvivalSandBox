using System.Linq;
using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Falling : Freefall {
        private Vector3 direction;
        
        private float currentSpeed;
        private float velocity;
        private float targetSpeed;
        private bool isPlayerOnGround;
        
        public Falling(StateMachine stateMachine) : base(stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_FALLING, true);
            velocity = 0;
            currentSpeed = movementStateMachine.lastSpeed;
            targetSpeed = movementStateMachine.GetSpeed * 0.1f;
        }

        public override void Update() {
            base.Update();
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            CheckForTransition();

            if (movementStateMachine.GetCharacterController.isGrounded) {
                Ray rayDown = new Ray(movementStateMachine.GetSphereCastPointGround.position, Vector3.down);
                bool sphereCast = Physics.SphereCast(rayDown, movementStateMachine.GetSlippingSphereCastRadius, out RaycastHit hitInfo, 0.2f);
                if (sphereCast) {
                    direction = hitInfo.normal - direction * -0.5f;
                    Vector3 vector3 = new Vector3(direction.x, movementStateMachine.GetSlippingSpeed, direction.z) * Time.deltaTime;
                    movementStateMachine.GetCharacterController.Move(vector3);
                    return;
                }
            }

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, 0.4f);
            movementStateMachine.Move(
                direction,
                currentSpeed,
                movementStateMachine.GetRotationSpeed * 0.1f,
                movementStateMachine.GetFallingBorder
            );
        }

        public override void Exit() {
            base.Exit();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_FALLING, false);
            movementStateMachine.lastSpeed = currentSpeed;
        }
        
        private void CheckForTransition() {
            isPlayerOnGround = Physics.OverlapSphere(movementStateMachine.GetGroundPoint.position,
                movementStateMachine.GetOnGroundSphereRadius).Any(x => !x.gameObject.CompareTag("Player"));
            if (isPlayerOnGround) {
                if (direction.magnitude > 0) {
                    if (movementStateMachine.isShiftPressed) {
                        movementStateMachine.ChangeState(movementStateMachine.runningState);
                        return;
                    }

                    movementStateMachine.ChangeState(movementStateMachine.joggingState);
                    return;
                }
                movementStateMachine.ChangeState(movementStateMachine.idleState);
            }
        }
    }
}