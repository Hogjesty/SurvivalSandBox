namespace Player.StateMachines.Movement.States {
    public class Freefall : BaseState {
        protected readonly MovementStateMachine movementStateMachine;
        
        protected Freefall(StateMachine stateMachine) : base(stateMachine) {
            movementStateMachine = (MovementStateMachine) this.stateMachine;
        }

        public override void Enter() {
        }

        public override void Update() {
        }

        public override void FixedUpdate() {
        }

        public override void Exit() {
        }
    }
}