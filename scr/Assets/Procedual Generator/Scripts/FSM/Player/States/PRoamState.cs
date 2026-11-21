using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRoamState : StateBase
{
    public PRoamState(Player _player) : base(_player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.SetDestination(player.GetNextRoomCentered(player.currentRoomIndex + 1));
        animator.SetTrigger("Walking");
    }
}