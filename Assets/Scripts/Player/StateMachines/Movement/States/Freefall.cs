namespace Player.StateMachines.Movement.States {
    public class Freefall : BaseState {
        protected readonly MovementStateMachine movementStateMachine;
        
        protected Freefall(string name, StateMachine stateMachine) : base(name, stateMachine) {
            movementStateMachine = (MovementStateMachine) this.stateMachine;
        }

        public override void Enter() {
        }

        public override void Update() {
        }

        public override void Exit() {
        }
    }
}