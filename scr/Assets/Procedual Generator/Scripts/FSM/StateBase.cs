using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateBase
{
    public List<Transition> transitions = new();

    public StateMachine fsm;
    protected readonly Player player;
    protected readonly Enemy enemy;
    protected readonly NavMeshAgent agent;
    protected readonly Animator animator;

    protected readonly Action<StateBase> onEnter;
    protected readonly Action<StateBase> onUpdate;
    protected readonly Action<StateBase> onExit;

    public StateBase(Player _player,
        Action<StateBase> _onEnter = null,
        Action<StateBase> _onLogic = null,
        Action<StateBase> _onExit = null)
    {
        this.player = _player;
        this.onEnter = _onEnter;
        this.onUpdate = _onLogic;
        this.onExit = _onExit;
        agent = _player.GetComponent<NavMeshAgent>();
        animator = _player.GetComponent<Animator>();
    }

    public StateBase(Enemy _enemy,
        Action<StateBase> _onEnter = null,
        Action<StateBase> _onLogic = null,
        Action<StateBase> _onExit = null)
    {
        this.enemy = _enemy;
        this.onEnter = _onEnter;
        this.onUpdate = _onLogic;
        this.onExit = _onExit;
        agent = _enemy.GetComponent<NavMeshAgent>();
        animator = _enemy.GetComponent<Animator>();
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public bool CheckSwap(out StateBase _nextState)
    {
        _nextState = null;
        foreach (Transition _transition in transitions)
        {
            if (_transition.Trigger())
            {
                _nextState = _transition.NextState;
                return true;
            }
        }

        return false;
    }

    public void AddTransitions(params Transition[] _transitions)
    {
        this.transitions.AddRange(_transitions);
    }
}

