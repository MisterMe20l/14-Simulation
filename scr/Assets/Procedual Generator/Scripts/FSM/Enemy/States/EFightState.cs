using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EFightState : StateBase
{
    private Sensors attackSensor;
    private SO_Enemies enemySO;
    private Player playerFSM;
    public bool Attacking;
    public bool Dying;

    public EFightState(Enemy _enemy, Sensors _attackSensor, Player _player, SO_Enemies _enemySO) : base(_enemy)
    {
        attackSensor = _attackSensor;
        playerFSM = _player;
        enemySO = _enemySO;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (attackSensor.TargetInside() && !Attacking)
        {
            Attacking = true;
            animator.SetTrigger("Attack");
            playerFSM.TakeDmg(enemySO.dmg);
        }
        if(enemy.Hp <= 0 && !Dying)
        {
            Dying = true;
            animator.SetTrigger("Dead");
        }
    }
}
