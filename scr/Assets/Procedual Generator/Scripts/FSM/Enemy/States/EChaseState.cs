using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EChaseState : StateBase
{
    private Transform playerPos;

    public EChaseState(Enemy _enemy, Transform _playerPos) : base(_enemy)
    {
        playerPos = _playerPos;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = false;
        agent.SetDestination(playerPos.position);
        animator.SetTrigger("Running");
    }
}
