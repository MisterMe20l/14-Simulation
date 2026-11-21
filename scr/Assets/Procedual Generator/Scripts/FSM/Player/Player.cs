using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SO_Player player;
    [SerializeField] private SO_Time clock;
    [SerializeField] internal SO_DungeonValues dungeonValues;
    [SerializeField] private LayerMask m_enemyMask;
    [SerializeField] private Vector3 m_shopPos;
    [SerializeField] private float m_disCloseToDestination;
    [SerializeField] private float m_disFarFromDestination;
    [SerializeField] internal List<GameObject> m_sword;
    [SerializeField] internal float m_playerSpeed;

    [Header("Sensors")]
    [SerializeField] private float m_attackSensorRadius;
    [SerializeField] private Color m_attackSensorColor;
    internal Sensors attackSensor;

    [Header("Particles")]
    [SerializeField] private float m_particlesDuration;
    [SerializeField] private float m_dTimeM; //Time.deltaTime multiplicator
    private ParticleSystem m_spawner;

    private Animator animator;
    private NavMeshAgent agent;
    private StateMachine playerFSM;

    internal List<GameObject> rooms;
    public List<GameObject> enemiesInRoom;
    internal RoomType currentRoomType;
    internal int currentRoomIndex;
    internal bool spawnAnimFinished;
    internal bool shoppingAnimFinished;
    internal bool wakeUpAnimFinished;
    internal bool lootAnimFinished;
    internal bool deathAnimFinished;
    internal bool isDay;
    internal bool leftLobby;
    internal bool onDestination;
    internal bool closeToDestination;
    internal bool farFromDestination;
    internal bool roomCleared;
    internal bool wakingUp;
    internal bool timerEnded;

    private bool nextFrame;

    #region FSM params

    private PSpawnState spawnState;
    private PSleepState sleepState;
    private PShoppingState shoppingState;
    private PNextRoomState nextRoomState;
    private PRoamState roamState;
    private PLootState lootState;
    private PFightState fightState;

    private void StartFSM()
    {
        //Add States:
        spawnState = new(this, player);
        sleepState = new(this);
        shoppingState = new(this, m_shopPos, player);
        nextRoomState = new(this);
        roamState = new(this);
        lootState = new(this);
        fightState = new(this, attackSensor, m_sword, player);

        playerFSM = new(spawnState);

        //Add Transitions:
        var spawnToSleep = new Transition(sleepState, () => spawnAnimFinished && !isDay);
        spawnState.AddTransitions(spawnToSleep);
        var spawnToShop = new Transition(shoppingState, () => spawnAnimFinished && isDay);
        spawnState.AddTransitions(spawnToShop);

        var sleepToShop = new Transition(shoppingState, () => isDay && wakeUpAnimFinished);
        sleepState.AddTransitions(sleepToShop);

        var shopToNext = new Transition(nextRoomState, () => shoppingAnimFinished);
        shoppingState.AddTransitions(shopToNext);

        var nextToSpawn = new Transition(spawnState, () => !isDay);
        nextRoomState.AddTransitions(nextToSpawn);
        var nextToRoam = new Transition(roamState, () => timerEnded && closeToDestination && currentRoomType == RoomType.Empty);
        nextRoomState.AddTransitions(nextToRoam);
        var nextToLoot = new Transition(lootState, () => timerEnded && closeToDestination && currentRoomType == RoomType.Loot);
        nextRoomState.AddTransitions(nextToLoot);
        var nextToFight = new Transition(fightState, () => timerEnded && closeToDestination && (currentRoomType == RoomType.Enemy || currentRoomType == RoomType.Boss));
        nextRoomState.AddTransitions(nextToFight);

        var roamToNext = new Transition(nextRoomState, () => !isDay || farFromDestination);
        roamState.AddTransitions(roamToNext);

        var lootToNext = new Transition(nextRoomState, () => !isDay || lootAnimFinished);
        lootState.AddTransitions(lootToNext);

        var fightToNext = new Transition(nextRoomState, () => !isDay || roomCleared);
        fightState.AddTransitions(fightToNext);
        var fightToSpawn = new Transition(spawnState, () => deathAnimFinished);
        fightState.AddTransitions(fightToSpawn);
    }

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        m_spawner = GetComponent<ParticleSystem>();

        attackSensor = new Sensors(transform, m_attackSensorRadius, m_enemyMask);

        enemiesInRoom = new();

        SetPlayerStats();

        StartFSM();
    }

    private void SetPlayerStats()
    {
        player.maxHealth = player.startMaxHealth;
        player.hp = player.maxHealth;
        player.dmg = player.startDmg;
        player.enemiesKilled = 0;
        player.lootedRooms = 0;
    }

    private void OnDrawGizmos()
    {
        DrawSensors();
        DrawDestinationPoints();
    }

    private void DrawSensors()
    {
        Gizmos.color = m_attackSensorColor;
        Gizmos.DrawWireSphere(transform.position, m_attackSensorRadius);
        //Gizmos.color = m_enemySensorColor;
        //Gizmos.DrawWireSphere(transform.position, m_enemySensorRadius);
    }

    private void DrawDestinationPoints()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(m_shopPos, .3f);
        var currentRoomPosCentered = (currentRoomIndex == -1) ? Vector3.zero : GetNextRoomCentered(currentRoomIndex);
        Gizmos.DrawSphere(currentRoomPosCentered, .3f);
        Gizmos.DrawWireSphere(currentRoomPosCentered, m_disCloseToDestination);
        var nextRoomPosCentered = (currentRoomIndex == -1) ? Vector3.zero : GetNextRoomCentered(currentRoomIndex + 1);
        Gizmos.DrawWireSphere(nextRoomPosCentered, m_disFarFromDestination);
    }

    private void Update()
    {
        GetBools();

        Debug.Log(playerFSM.currentState);

        playerFSM.Update();
    }

    //Check Transition's bools:
    private void GetBools()
    {
        onDestination = agent.remainingDistance <= agent.stoppingDistance;
        closeToDestination = agent.remainingDistance <= m_disCloseToDestination;
        farFromDestination = agent.remainingDistance <= m_disFarFromDestination;
        isDay = IsDay();
    }

    private bool IsDay()
    {
        var isDay = clock.Hour >= 6 && clock.Hour < 22;
        return isDay;
    }

    internal void RespawnPlayer()
    {
        StartCoroutine(SpawnRoutine());
    }

    internal int GetRndAttack()
    {
        return Random.Range(1, 4);
    }

    private IEnumerator SpawnRoutine()
    {
        m_spawner.Play();
        m_spawner.emissionRate = 0f;
        var timer = 0f;
        while (timer < m_particlesDuration)
        {
            m_spawner.emissionRate += 2f * (m_dTimeM * Time.deltaTime);
            transform.position = Vector3.zero;
            timer += Time.deltaTime;
            yield return null;
        }
        while (m_spawner.emissionRate > 5f)
        {
            m_spawner.emissionRate -= 5f * (m_dTimeM * Time.deltaTime);
            transform.position = Vector3.zero;
            timer += Time.deltaTime;
            yield return null;
        }
        m_spawner.emissionRate = 0f;
        yield return new WaitForSeconds(2);
        spawnAnimFinished = true;
        m_spawner.Stop();
    }

    internal void StartTimer(float _timerDuration)
    {
        StartCoroutine(TimerRoutine(_timerDuration));
    }

    private IEnumerator TimerRoutine(float _timerDuration)
    {
        timerEnded = false;
        var timer = 0f;
        while (timer < _timerDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timerEnded = true;
    }

    internal void GetRoomList()
    {
        rooms = dungeonValues.rooms;
        currentRoomIndex = -1;
    }

    internal Vector3 GetNextRoomCentered(int _index)
    {
        var toReturn = Vector3.zero;
        if (playerFSM != null)
        {
            if (playerFSM.currentState == nextRoomState) toReturn = dungeonValues.rooms[_index].transform.position + new Vector3(1.5f, 0f, 1.5f);
        }
        return toReturn;
    }

    public void TakeDmg(int _dmg)
    {
        player.hp -= _dmg;
    }

    public void EnemyDead(GameObject _enemy)
    {
        enemiesInRoom.Remove(_enemy);
        player.enemiesKilled++;
    }

    #region Animation Events

    public void ShoppingAnimFinished()
    {
        shoppingAnimFinished = true;
    }

    public void WakeUpAnimFinished()
    {
        wakeUpAnimFinished = true;
    }

    public void LootAnimFinished()
    {
        lootAnimFinished = true;
        player.lootedRooms++;
    }

    public void AttackAnimFinished()
    {
        fightState.Attacking = false;
    }

    public void DeathAnimFinished()
    {
        deathAnimFinished = true;
        fightState.Attacking = false;
        fightState.Dying = false;
    }

    #endregion
}
