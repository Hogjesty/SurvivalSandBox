namespace Player.StateMachines {
    public abstract class BaseState {
        public readonly string name;

        protected readonly StateMachine stateMachine;

        protected BaseState(StateMachine stateMachine) {
            name = GetType().Name;
            this.stateMachine = stateMachine;
        }

        public abstract void Enter();

        public abstract void Update();
        
        public abstract void FixedUpdate();

        public abstract void Exit();
    }
}