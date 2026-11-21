using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNextRoomState : StateBase
{
    public PNextRoomState(Player _player) : base(_player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.currentRoomIndex ++;
        var index = player.dungeonValues.roomIndexPath[player.currentRoomIndex];
        player.currentRoomType = player.rooms[index].GetComponent<RoomBehavior>().m_Type;
        agent.isStopped = false;
        agent.speed = player.m_playerSpeed;
        agent.SetDestination(player.GetNextRoomCentered(index));
        animator.SetTrigger("Running");
        player.StartTimer(1);
    }

    public override void OnExit()
    {
        base.OnExit();
        agent.speed = player.m_playerSpeed / 2;
    }
}
