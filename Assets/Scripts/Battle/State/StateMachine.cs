
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    StateBase currentState;

    StateBase previousState;

    public StateMachine()
    {

    }

    public void Excute()
    {
        if (currentState != null)
        {
            currentState.Excute();
        }
    }

    public void ChangeState(StateBase _newState, params object[] param)
    {
        if (currentState != null)
        {
            currentState.ExitExcute();
            previousState = currentState;
        }
        currentState = _newState;
        currentState.SetParam(param);
        currentState.EnterExcute();
    }

    public void SetCurrentState(StateBase _currentState)
    {
        currentState = _currentState;
        currentState.EnterExcute();
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    public StateBase GetCurrentState()
    {
        return currentState;
    }

    public StateBase GetPreviousState()
    {
        return previousState;
    }

}

