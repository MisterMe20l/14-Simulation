using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PSleepState : StateBase
{
    public PSleepState(Player _player) : base(_player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        player.wakingUp = false;
        animator.SetTrigger("Sleep");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.isDay && !player.wakingUp)
        {
            player.wakingUp = true;
            animator.SetTrigger("WakeUp");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
