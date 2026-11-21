using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private SO_Enemies enemy;
    [SerializeField] private SO_Time clock;
    [SerializeField] private LayerMask m_playerMask;

    [Space]
    [Header("Sensors")]
    [SerializeField] private float m_attackSensorRadius;
    [SerializeField] private Color m_attackSensorColor;
    private Sensors attackSensor;

    private Animator animator;
    private NavMeshAgent agent;
    private StateMachine enemyFSM;
    private GameObject player;

    private bool playerInRoom;
    public int Hp;
    private int dmg;

    #region FSM params

    private EIdleState idleState;
    private EChaseState chaseState;
    private EFightState fightState;
    private EDeadState deadState;

    private void StartFSM()
    {
        //Add States:
        idleState = new(this);
        chaseState = new(this, player.GetComponent<Transform>());
        fightState = new(this, attackSensor, player.GetComponent<Player>(), enemy);
        deadState = new(this);

        enemyFSM = new(idleState);

        //Add Transitions:
        var idleToChase = new Transition(chaseState, () => playerInRoom);
        idleState.AddTransitions(idleToChase);

        var chaseToFight = new Transition(fightState, () => attackSensor.TargetInside());
        chaseState.AddTransitions(chaseToFight);

        var fightToIdle = new Transition(idleState, () => false);
        fightState.AddTransitions(fightToIdle);
        var fightToDead = new Transition(deadState, () => false);
        fightState.AddTransitions(fightToDead);
    }

    #endregion

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        animator = GetComponent<Animator>();

        attackSensor = new(transform, m_attackSensorRadius, m_playerMask);
        playerInRoom = false;

        player = GameObject.FindGameObjectWithTag("Player");

        SetValues();

        StartFSM();
    }

    private void SetValues()
    {
        Hp = enemy.maxHealth + (clock.Day - 1) * 5;
        dmg = enemy.dmg + (clock.Day - 1) * 5;
    }

    private void OnDrawGizmos()
    {
        DrawSensors();
    }

    private void DrawSensors()
    {
        Gizmos.color = m_attackSensorColor;
        Gizmos.DrawWireSphere(transform.position, m_attackSensorRadius);
    }

    private void Update()
    {
        enemyFSM.Update();

        //Debug.Log(enemyFSM.currentState);
    }

    public void TakeDmg(int _dmg)
    {
        Hp -= _dmg;
    }

    public void EnteredRoom()
    {
        playerInRoom = true;
    }

    public void PlayerDead()
    {
        playerInRoom = true;
    }

    #region Animation Events

    public void AttackAnimFinished()
    {
        fightState.Attacking = false;
    }

    public void DeathAnimEnded()
    {
        player.GetComponent<Player>().EnemyDead(gameObject);
        Destroy(gameObject);
    }

    #endregion
}
