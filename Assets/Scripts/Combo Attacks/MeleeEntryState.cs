using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntryState : State
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        // if player is in air: State nextState = (State) new AirEntryState();
        State nextState = (State)new GroundEntryState();
        stateMachine.SetNextState(nextState);
    }
}
