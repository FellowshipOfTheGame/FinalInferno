using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{

    public State currentState;

    [HideInInspector] public float stateTimeElapsed;

    [SerializeField] private Action[] startActions;

    void Start()
    {
        foreach (Action A in startActions)
        {
            A.Act(this);
        }
    }

    void Update()
    {
        stateTimeElapsed += Time.deltaTime;
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState, Action[] transitionActions)
    {
        OnExitState(transitionActions);
        currentState = nextState;
        Debug.Log("New State: " + nextState.name);
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState(Action[] transitionActions)
    {
        stateTimeElapsed = 0;
        foreach (Action A in transitionActions)
        {
            A.Act(this);
        }
    }
}
