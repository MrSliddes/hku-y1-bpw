using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractFSMState : ScriptableObject
{
    protected NavMeshAgent navMeshAgent;
    protected FSMEnemy enemy;
    protected FiniteStateMachine finiteStateMachine;

    public ExecutionState executionState { get; protected set; }
    public FSMStateType stateType { get; protected set; }
    public bool enteredState { get; protected set; }

    public virtual void OnEnable()
    {
        executionState = ExecutionState.none;
    }

    public virtual bool EnterState()
    {
        bool succesNavMesh = true;
        bool succesEnemy = true;
        executionState = ExecutionState.active;

        // Check if navmeshagent exists
        succesNavMesh = (navMeshAgent != null);

        // Check if executing agent exists
        succesEnemy = (enemy != null);

        return succesNavMesh & succesEnemy;
    }

    public abstract void UpdateState();

    public virtual bool ExitState()
    {
        executionState = ExecutionState.completed;
        return true;
    }

    public virtual void SetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        if(navMeshAgent != null)
        {
            this.navMeshAgent = navMeshAgent;
        }
    }

    public virtual void SetExecutingFSM(FiniteStateMachine fsm)
    {
        if(fsm != null)
        {
            this.finiteStateMachine = fsm;
        }
    }

    public virtual void SetExecutingEnemy(FSMEnemy enemy)
    {
        if(enemy != null)
        {
            this.enemy = enemy;
        }
    }
}

public enum ExecutionState
{
    none,
    active,
    completed,
    terminated
}

public enum FSMStateType
{
    idle,
    chase
}
