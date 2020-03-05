using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "Unity-FSM/States/Idle", order = 1)]
public class IdleState : AbstractFSMState
{
    [SerializeField]
    float idleDuration = 3f;

    float totalDuration;

    public override void OnEnable()
    {
        base.OnEnable();
        stateType = FSMStateType.idle;
    }

    public override bool EnterState()
    {
        enteredState = base.EnterState();
        
        if(enteredState)
        {
            Debug.Log("Enter idle state");
            totalDuration = 0f;
        }

        return enteredState;
    }

    public override void UpdateState()
    {
        if (enteredState)
        {
            Debug.Log("Update idle state");

            totalDuration += Time.deltaTime; 

            if(totalDuration >= idleDuration)
            {
                // Go to next state
                finiteStateMachine.EnterState(FSMStateType.chase);
            }
        }

    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("Exit idle state");
        return true;
    }
}
