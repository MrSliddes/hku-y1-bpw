using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine))]
public class FSMEnemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    FiniteStateMachine finiteStateMachine;

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        finiteStateMachine = GetComponent<FiniteStateMachine>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
