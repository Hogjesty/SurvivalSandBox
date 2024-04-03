namespace Player.StateMachines {
    public abstract class BaseState {
        public readonly string name;

        protected readonly StateMachine stateMachine;

        protected BaseState(string name, StateMachine stateMachine) {
            this.name = name;
            this.stateMachine = stateMachine;
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();
    }
}