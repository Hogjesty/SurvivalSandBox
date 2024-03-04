using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;

public class Player : MonoBehaviour {

    private StateMachine playerStateMachine;

    private readonly State idleState = new Idle();
    private readonly State walkState = new Walk();
    private readonly State jumpState = new Jump();
    private readonly State rotationState = new Rotation();
    
    void Start() {
        playerStateMachine = new StateMachine(idleState);
    }

    void Update() {
        if (Input.GetKey(KeyCode.R)) {
            playerStateMachine.ToggleState(rotationState);
        }
        
        if (Input.GetKey(KeyCode.W)) {
            playerStateMachine.ToggleState(walkState);
        }
        
        

        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     playerStateMachine.ToggleState(jumpState);
        // }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.R)) {
            playerStateMachine.ToggleState(idleState);
        }
        
        playerStateMachine.currentState.Update(this);
    }
}