using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField]
    AbstractFSMState startingState;

    AbstractFSMState currentState;

    [SerializeField]
    List<AbstractFSMState> validStates;
    Dictionary<FSMStateType, AbstractFSMState> fsmStates;

    public void Awake()
    {
        currentState = null;

        fsmStates = new Dictionary<FSMStateType, AbstractFSMState>();
        NavMeshAgent navMeshAgent = this.GetComponent<NavMeshAgent>();
        FSMEnemy enemy = this.GetComponent<FSMEnemy>();

        foreach(AbstractFSMState state in validStates)
        {
            state.SetExecutingFSM(this);
            state.SetExecutingEnemy(enemy);
            state.SetNavMeshAgent(navMeshAgent);
            fsmStates.Add(state.stateType, state);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(startingState != null)
        {
            EnterState(startingState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateState();
        }
    }

    #region State managment

    public void EnterState(AbstractFSMState nextState)
    {
        if (nextState == null) return;

        if(currentState != null)
        {
            currentState.ExitState();
        }

        currentState = nextState;
        currentState.EnterState();
    }

    public void EnterState(FSMStateType stateType)
    {
        if(fsmStates.ContainsKey(stateType))
        {
            AbstractFSMState nextState = fsmStates[stateType];

            currentState.ExitState();
            EnterState(nextState);
        }
    }

    #endregion
}
