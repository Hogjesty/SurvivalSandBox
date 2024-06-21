using System.Linq;
using UnityEngine;

namespace Player.StateMachines.Movement.States.SubStates {
    public class Slipping : BaseState {
        private readonly MovementStateMachine movementStateMachine;
        
        private RaycastHit slippingPointInfo;
        private bool isPlayerOnGround;

        public Slipping(StateMachine stateMachine) : base(stateMachine) {
            movementStateMachine = (MovementStateMachine) this.stateMachine;
        }

        public override void Enter() {
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_SLIPPING, true);
        }

        public override void Update() {
            CheckForTransition();
            
            CharacterController cont = movementStateMachine.GetCharacterController;
            
            Vector3 slopeDirection = Vector3.Cross(slippingPointInfo.normal, Vector3.down);
            slopeDirection = Vector3.Cross(slopeDirection, slippingPointInfo.normal);
            cont.Move(slopeDirection.normalized * Time.deltaTime);
        }

        public override void FixedUpdate() {
            isPlayerOnGround = Physics.OverlapSphere(movementStateMachine.GetGroundPoint.position, 0.15f)
                .Any(x => !x.gameObject.CompareTag("Player"));
            
            Ray rayDown = new Ray(movementStateMachine.GetSphereCastPointGround.position, Vector3.down);
            Physics.SphereCast(rayDown, 0.26f, out RaycastHit hitInfo, 0.3f);
            slippingPointInfo = hitInfo;
        }

        public override void Exit() {
            movementStateMachine.GetPlayerAnimator.SetBool(MovementStateMachine.IS_SLIPPING, false);
        }

        private void CheckForTransition() {
            if (isPlayerOnGround) {
                movementStateMachine.ChangeState(movementStateMachine.idleState);
            }
        }
    }
}