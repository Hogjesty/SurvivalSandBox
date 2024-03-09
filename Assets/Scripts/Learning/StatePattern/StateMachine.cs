using System;

public class StateMachine {
    public State currentState { get; set; }

    public StateMachine(State startState) {
        currentState = startState;
        currentState.Enter();
    }

    public void ToggleState(State state) {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}