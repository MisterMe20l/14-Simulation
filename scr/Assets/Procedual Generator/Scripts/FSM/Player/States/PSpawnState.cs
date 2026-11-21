using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSpawnState : StateBase
{
    private SO_Player playerSO;

    public PSpawnState(Player _player, SO_Player _playerSO) : base(_player)
    {
        playerSO = _playerSO;
    }

    public override void OnEnter()
    {
        player.deathAnimFinished = false;
        player.spawnAnimFinished = false;
        base.OnEnter();
        agent.isStopped = true;
        player.RespawnPlayer();
        player.transform.position = new(0f, 0f, 0f);
        animator.SetTrigger("Respawn");
        playerSO.hp = playerSO.maxHealth;
    }
}
