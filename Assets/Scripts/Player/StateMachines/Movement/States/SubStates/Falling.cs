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
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            base.Update();
            CheckForTransition();

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, 0.4f);
            movementStateMachine.Move(
                direction,
                currentSpeed,
                movementStateMachine.GetRotationSpeed * 0.1f,
                movementStateMachine.GetFallingBorder
            );
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            isPlayerOnGround = Physics.OverlapSphere(movementStateMachine.GetGroundPoint.position, 0.15f)
                .Any(x => !x.gameObject.CompareTag("Player"));
        }

        public override void Exit() {
            base.Exit();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_FALLING, false);
            movementStateMachine.lastSpeed = currentSpeed;
        }
        
        private void CheckForTransition() {
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

            // CharacterController cont = movementStateMachine.GetCharacterController;
            //
            // if (cont.isGrounded) {
            //     RaycastHit hit;
            //     if (Physics.Raycast(movementStateMachine.GetGroundPoint.position, Vector3.down, out hit)) {
            //         float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            //         if (slopeAngle > cont.slopeLimit) {
            //             Vector3 slopeDirection = Vector3.Cross(hit.normal, Vector3.down);
            //             slopeDirection = Vector3.Cross(slopeDirection, hit.normal);
            //             cont.Move(slopeDirection.normalized * 1 * Time.deltaTime);
            //         }
            //     }
            // }
        }
    }
}