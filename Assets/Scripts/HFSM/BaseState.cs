namespace HFSM {
    public class BaseState {
        public readonly string name;

        protected readonly StateMachine stateMachine;

        protected BaseState(string name, StateMachine stateMachine) {
            this.name = name;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() {
        }

        public virtual void Update() {
        }

        public virtual void Exit() {
        }
    }
}