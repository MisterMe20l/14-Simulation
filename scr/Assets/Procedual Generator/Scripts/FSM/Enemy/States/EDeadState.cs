using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDeadState : StateBase
{
    public EDeadState(Enemy _enemy) : base(_enemy)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        animator.SetTrigger("Dead");
    }
}
