using UnityEngine;

namespace Player.StateMachines {
    public abstract class StateMachine : MonoBehaviour {
        private BaseState currentState;
        protected BaseState CurrentState => currentState;

        private void Start() {
            currentState = GetInitialState();
            currentState?.Enter();
        }

        private void Update() {
            currentState?.Update();
        }

        private void FixedUpdate() {
            currentState?.FixedUpdate();
        }

        protected abstract BaseState GetInitialState();

        public void ChangeState(BaseState newState) {
            currentState.Exit();
            currentState = newState;
            newState.Enter();
        }
    }
}