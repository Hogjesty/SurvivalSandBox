using Learning.HFSM.Movement.States.SubStates;
using UnityEngine;

namespace Learning.HFSM.Movement {
    public class MovementSM : StateMachine {
        [HideInInspector] public Idle idleState;
        
        [HideInInspector] public Jogging joggingState;
        [HideInInspector] public Running runningState;
        
        [HideInInspector] public Jumping jumpingState;
        [HideInInspector] public Falling fallingState;

        private void Awake() {
            idleState = new Idle(this);
            joggingState = new Jogging(this);
            runningState = new Running(this);
            jumpingState = new Jumping(this);
            fallingState = new Falling(this);
        }

        protected override BaseState GetInitialState() {
            return idleState;
        }
    }
}