using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase State", menuName = "Unity-FSM/States/Chase", order = 2)]
public class ChaseState : AbstractFSMState
{
    public override void OnEnable()
    {
        base.OnEnable();
        stateType = FSMStateType.chase;
    }

    public override bool EnterState()
    {
        enteredState = true;

        return enteredState;
    }

    public override void UpdateState()
    {
        // Chase target
        if(enteredState)
        {
            // Close enough to target?
            if(Vector3.Distance(navMeshAgent.transform.position, GameObject.FindWithTag("Player").transform.position) < 1)
            {
                finiteStateMachine.EnterState(FSMStateType.idle);
            }

            // Chase
            SetDestination(GameObject.FindWithTag("Player").transform); // In bigger projects it is bad to cast this every time, its better to get the player transfrom once and store it
        }

    }

    private void SetDestination(Transform target)
    {
        if(navMeshAgent != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
}
