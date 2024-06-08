using UnityEngine;

namespace Player.StateMachines.Interaction.States.SubStates {
    public class Idle : BaseState {
        protected readonly InteractionStateMachine interactionStateMachine;
        
        public Idle(StateMachine stateMachine) : base(stateMachine) {
            interactionStateMachine = (InteractionStateMachine)this.stateMachine;
        }

        public override void Enter() {
            interactionStateMachine.currentObject = null;
        }

        public override void Update() {
            bool raycast = Physics.Raycast(
                interactionStateMachine.StartRayPoint.position,
                interactionStateMachine.MainCamera.transform.forward,
                out RaycastHit hitInfo,
                2f,
                interactionStateMachine.LayerMask
            );
            if (raycast) {
                Vector3 objPos = hitInfo.transform.position;
                Vector3 objPosPixels = interactionStateMachine.MainCamera.WorldToViewportPoint(objPos);
                objPosPixels.y += Screen.height * 0.1f;
                interactionStateMachine.InteractPopupText.transform.localPosition = objPosPixels;
                interactionStateMachine.InteractPopupText.gameObject.SetActive(true);
                
                if (Input.GetKeyDown(KeyCode.E)) {
                    interactionStateMachine.currentObject = hitInfo.transform.gameObject;
                    if (interactionStateMachine.currentObject.CompareTag("Inventory")) {
                        interactionStateMachine.ChangeState(interactionStateMachine.openingState);
                    } else {
                        interactionStateMachine.ChangeState(interactionStateMachine.pickingUpState);
                    }
                    interactionStateMachine.InteractPopupText.gameObject.SetActive(false);
                }
                
            } else {
                interactionStateMachine.InteractPopupText.gameObject.SetActive(false);
            }
        }

        public override void Exit() {
            
        }
    }
}