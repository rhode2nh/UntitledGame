using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="IdleState", menuName ="Unity-FSM/States/Idle", order =1)]
public class IdleState : AbstractFSMState
{
    public override bool EnterState()
    {
        base.EnterState();
        Debug.Log("ENTERED IDLE STATE");
        return true;
    }

    public override void UpdateState()
    {
        Debug.Log("UPDATING STATE");
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("ENTERED EXIT STATE");
        return true;
    }
}
