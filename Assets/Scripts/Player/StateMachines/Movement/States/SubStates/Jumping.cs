using System.Linq;
using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Jumping : Freefall {
        private Vector3 direction;

        private float gravity;
        private bool readyToLand;
        private float currentSpeed;
        private float velocity;
        private float targetSpeed;
        private bool isPlayerOnGround;
        
        public Jumping(StateMachine stateMachine) : base(stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_JUMPING, true);
            readyToLand = false;
            gravity = movementStateMachine.GetJumpForce;
            velocity = 0;
            currentSpeed = movementStateMachine.lastSpeed;
            targetSpeed = movementStateMachine.GetSpeed;
        }

        public override void Update() {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            base.Update();
            CheckForTransition();
            
            gravity -= Time.deltaTime * movementStateMachine.GetGravitySpeed;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, 0.5f);
            movementStateMachine.Move(
                direction,
                currentSpeed,
                movementStateMachine.GetRotationSpeed * 0.8f,
                gravity
            );
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            isPlayerOnGround = Physics.OverlapSphere(movementStateMachine.GetGroundPoint.position, 0.15f)
                .Any(x => !x.gameObject.CompareTag("Player"));
        }

        public override void Exit() {
            base.Exit();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_JUMPING, false);
            movementStateMachine.lastSpeed = currentSpeed;
        }
        
        private void CheckForTransition() {
            if (!isPlayerOnGround) {
                readyToLand = true;
            }
            
            if (isPlayerOnGround && readyToLand) {
                readyToLand = false;
                if (direction.magnitude > 0) {
                    if (movementStateMachine.isShiftPressed) {
                        movementStateMachine.ChangeState(movementStateMachine.runningState);
                        return;
                    }

                    movementStateMachine.ChangeState(movementStateMachine.joggingState);
                    return;
                }
                movementStateMachine.ChangeState(movementStateMachine.idleState);
                return;
            }

            if (gravity <= movementStateMachine.GetFallingBorder) {
                movementStateMachine.ChangeState(movementStateMachine.fallingState);
            }
        }
    }
}