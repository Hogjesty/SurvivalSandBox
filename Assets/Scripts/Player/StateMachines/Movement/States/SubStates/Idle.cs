using Player.StateMachines.Combat.States.SubStates.Crossbow;
using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Idle: AdaptiveGrounded  {
        public Idle(StateMachine stateMachine) : base(stateMachine) {
        }

        public override void Enter() {
            base.Enter();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_IDLE, true);
            movementStateMachine.isShiftPressed = false;
        }

        public override void Update() {
            base.Update();
            if (movementStateMachine.GetStateMachineManager.GetCombatCurrentStateName().Equals(nameof(CrossbowHolding))) {
                movementStateMachine.Rotate(movementStateMachine.GetRotationSpeed);
            }

            if (new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).magnitude > 0) {
                movementStateMachine.ChangeState(movementStateMachine.joggingState);
            }
        }

        public override void Exit() {
            base.Exit();
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_IDLE, false);
            movementStateMachine.lastSpeed = 0;
        }
    }
}