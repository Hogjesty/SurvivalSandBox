using UnityEngine;

namespace Player.StateMachines.Interaction.States.SubStates {
    public class Idle : BaseState {
        protected readonly InteractionStateMachine interactionStateMachine;
        
        public Idle(StateMachine stateMachine) : base("Idle", stateMachine) {
            interactionStateMachine = (InteractionStateMachine)this.stateMachine;
        }

        public override void Enter() {
            interactionStateMachine.currentObject = null;
        }

        public override void Update() {
            bool raycast = Physics.Raycast(
                interactionStateMachine.GetStartRayPoint.position,
                interactionStateMachine.GetMainCamera.transform.forward,
                out RaycastHit hitInfo,
                2f,
                interactionStateMachine.GetLayerMask
            );
            if (raycast) {
                Vector3 objPos = hitInfo.transform.position;
                Vector3 objPosPixels = interactionStateMachine.GetMainCamera.WorldToViewportPoint(objPos);
                objPosPixels.y += Screen.height * 0.1f;
                interactionStateMachine.GetInteractPopupText.transform.localPosition = objPosPixels;
                interactionStateMachine.GetInteractPopupText.gameObject.SetActive(true);
                
                if (Input.GetKeyDown(KeyCode.E)) {
                    interactionStateMachine.currentObject = hitInfo.transform.gameObject;
                    interactionStateMachine.ChangeState(interactionStateMachine.pickingUpState);
                    interactionStateMachine.GetInteractPopupText.gameObject.SetActive(false);
                }
                
            } else {
                interactionStateMachine.GetInteractPopupText.gameObject.SetActive(false);
            }
        }

        public override void Exit() {
            
        }
    }
}