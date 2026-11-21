using System;

public class Transition
{
    public Transition(StateBase _nextState, Func<bool> _trigger)
    {
        NextState = _nextState;
        Trigger = _trigger;
    }

    public StateBase NextState { get; private set; }

    public Func<bool> Trigger { get; private set; }
}

