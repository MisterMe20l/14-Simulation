using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShoppingState : StateBase
{
    private Vector3 shopPos;
    private bool shopReached;
    private SO_Player playerSO;

    public PShoppingState(Player _player, Vector3 _shopPos, SO_Player _playerSO) : base(_player)
    {
        shopPos = _shopPos;
        playerSO = _playerSO;
    }

    public override void OnEnter()
    {
        player.shoppingAnimFinished = false;
        agent.isStopped = false;
        player.onDestination = false;
        shopReached = false;
        agent.SetDestination(shopPos);
        agent.speed = player.m_playerSpeed;
        animator.SetTrigger("Running");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.onDestination && !shopReached)
        {
            shopReached = true;
            agent.isStopped = true;
            animator.SetTrigger("Interact");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        playerSO.maxHealth += 5 * playerSO.lootedRooms;
        playerSO.hp = playerSO.maxHealth;
        playerSO.dmg += playerSO.enemiesKilled;
        playerSO.lootedRooms = 0;
        playerSO.enemiesKilled = 0;
        player.GetRoomList();
    }
}
