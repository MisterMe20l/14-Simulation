using System.Collections.Generic;

public class StateMachine
{
    private List<Transition> anyStateTransitions = new();
    public StateBase currentState;

    public StateMachine(StateBase _initialState)
    {
        currentState = _initialState;
        currentState.OnEnter();
    }

    public void Update()
    {
        var swapped = false;
        while (CheckSwap(out StateBase _nextState) && currentState != _nextState)
        {
            currentState.OnExit();
            currentState = _nextState;
            currentState.OnEnter();
            swapped = true;
            break;
        }

        while (currentState.CheckSwap(out StateBase _nextState))
        {
            currentState.OnExit();
            currentState = _nextState;
            currentState.OnEnter();
            swapped = true;
            break;
        }
        if (swapped) return;

        currentState.OnUpdate();
    }

    public bool CheckSwap(out StateBase _nextState)
    {
        _nextState = null;
        foreach (Transition _transition in anyStateTransitions)
        {
            if (_transition.Trigger())
            {
                _nextState = _transition.NextState;
                return true;
            }
        }

        return false;
    }

    public void AddAnyStateTransitions(params Transition[] _transitions)
    {
        this.anyStateTransitions.AddRange(_transitions);
    }
}
