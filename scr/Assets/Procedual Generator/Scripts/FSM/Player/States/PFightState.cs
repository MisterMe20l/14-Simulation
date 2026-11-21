using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFightState : StateBase
{
    private Sensors attackSensor;
    private List<GameObject> swords;
    private SO_Player playerSO;
    public bool Attacking;
    public bool Dying;

    public PFightState(Player _player, Sensors _attackSensor, List<GameObject> _swords, SO_Player _playerSO) : base(_player)
    {
        attackSensor = _attackSensor;
        swords = _swords;
        playerSO = _playerSO;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        player.roomCleared = false;
        for (int i = 0; i < swords.Count; i++)
        {
            swords[i].SetActive(true);
        }
        player.enemiesInRoom = new();
        foreach (GameObject _enemy in player.rooms[player.currentRoomIndex].GetComponent<EnemyManager>().EnemiesInRoom)
        {
            player.enemiesInRoom.Add(_enemy);
            _enemy.GetComponent<Enemy>().EnteredRoom();
        }
        animator.SetTrigger("Fight");
        Attacking = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var enemyInside = player.attackSensor.TargetInside();
        if (enemyInside && !Attacking)
        {
            Attacking = true;
            animator.SetInteger("AttackIndex", player.GetRndAttack());
            animator.SetTrigger("Attack");
            for (int i = 0; i < player.enemiesInRoom.Count; i++)
            {
                player.enemiesInRoom[i].GetComponent<Enemy>().TakeDmg(playerSO.dmg);
            }
        }
        animator.SetBool("Attacking", Attacking);
        if (playerSO.hp <= 0 && !Dying)
        {
            Dying = true;
            animator.SetTrigger("Dead");
            for (int i = 0; i < player.enemiesInRoom.Count; i++)
            {
                player.enemiesInRoom[i].GetComponent<Enemy>().PlayerDead();
            }
        }
        if (player.enemiesInRoom.Count == 0) player.roomCleared = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        for (int i = 0; i < swords.Count; i++)
        {
            swords[i].SetActive(false);
        }
        player.roomCleared = false;
    }
}
