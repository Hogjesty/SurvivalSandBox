using Inventory;
using Player.AnimationsEvents;
using UnityEngine;

namespace Player.StateMachines.Interaction.States.SubStates {
    public class PickingUp : BaseState {
        protected readonly InteractionStateMachine interactionStateMachine;
        
        private bool isAnimationEnded;
        
        public PickingUp(StateMachine stateMachine) : base("PickingUp", stateMachine) {
            interactionStateMachine = (InteractionStateMachine)this.stateMachine;
            PlayerAnimationsEvents.endOfPickingUpAnim += EndOfPickingUpAnim;
        }

        public override void Enter() {
            interactionStateMachine.SetAnimBool(interactionStateMachine.IsPickingUpBool, true);
            isAnimationEnded = false;
        }

        public override void Update() {
            Transform playerTransform = interactionStateMachine.transform;
            Vector3 directionToStone = interactionStateMachine.currentObject.transform.position - playerTransform.position;
            directionToStone.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToStone);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, 0.05f);
            if (isAnimationEnded) {
                interactionStateMachine.ChangeState(interactionStateMachine.idleState);
            }
        }

        public override void Exit() {
            interactionStateMachine.SetAnimBool(interactionStateMachine.IsPickingUpBool, false);
            interactionStateMachine.GetPlayerInventory.TryToAddItem(interactionStateMachine.currentObject.GetComponent<WorldItem>()); 
            interactionStateMachine.DestroyCurrentObject();
        }

        private void EndOfPickingUpAnim() {
            isAnimationEnded = true;
        }
        
        ~PickingUp() {
            PlayerAnimationsEvents.endOfPickingUpAnim -= EndOfPickingUpAnim;
        }
    }
}