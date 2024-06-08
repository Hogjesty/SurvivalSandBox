using Inventory;

namespace Player.StateMachines.Interaction.States.SubStates {
    public class Opening : BaseState {
        protected readonly InteractionStateMachine interactionStateMachine;
        
        public Opening(StateMachine stateMachine) : base(stateMachine) {
            interactionStateMachine = (InteractionStateMachine)this.stateMachine;
        }

        public override void Enter() {
            IStorage storage = interactionStateMachine.currentObject.GetComponent<IStorage>();
            interactionStateMachine.InventoriesManager.OpenStorage(storage.GetStorage(), storage.GetStorageType());
        }

        public override void Update() {
            interactionStateMachine.ChangeState(interactionStateMachine.idleState);
        }

        public override void Exit() {
            
        }
    }
}