using UnityEngine;

namespace HFSM {
    public abstract class StateMachine : MonoBehaviour {
        private BaseState currentState;

        private void Start() {
            currentState = GetInitialState();
            currentState?.Enter();
        }

        private void Update() {
            currentState?.Update();
        }

        protected abstract BaseState GetInitialState();

        public void ChangeState(BaseState newState) {
            currentState.Exit();
            currentState = newState;
            newState.Enter();
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
            string content = currentState != null ? currentState.name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }
    }
}