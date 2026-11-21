using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EIdleState : StateBase
{
    public EIdleState(Enemy _enemy) : base(_enemy)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }
}
