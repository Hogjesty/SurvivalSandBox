using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : State {
    
    public override void Enter() {
        base.Enter();
        //Trow player up for 10 power points
        //ToggleState(Falling)
    }

    public override void Exit() {
        base.Exit();
    }
}
