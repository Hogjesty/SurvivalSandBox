using System.Linq;
using UnityEngine;

namespace Player.StateMachines.Movement.States {
    public class Grounded : BaseState {
        protected readonly MovementStateMachine movementStateMachine;

        protected Grounded(StateMachine stateMachine) : base(stateMachine) {
            movementStateMachine = (MovementStateMachine) this.stateMachine;
        }

        public override void Enter() {
        }

        public override void Update() {
        }

        public override void FixedUpdate() {
            bool isPlayerOnGround = Physics.OverlapSphere(movementStateMachine.GetGroundPoint.position, 0.15f)
                .Any(x => !x.gameObject.CompareTag("Player"));
            if (!isPlayerOnGround) {
                movementStateMachine.ChangeState(movementStateMachine.fallingState);
            }
        }

        public override void Exit() {
        }
    }
}