using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Values")]
public class SO_DungeonValues : ScriptableObject
{
    public Vector2 size;
    public Vector2 offset;
    public Vector3 startPos;
    public Vector3 lastRoomPos;
    public bool generated;
    public List<GameObject> rooms;
    public List<int> roomIndexPath;
    public NavMeshSurface navMesh;

    [Space]
    public bool instantiate;
    [Space]
    public GameObject emptyRoomPrefab;
    public Color emptyGizmosColor;
    public int emptyRoomRate;
    [Space]
    public GameObject lootRoomPrefab;
    public Color lootGizmosColor;
    public int lootRoomRate;
    [Space]
    public GameObject enemyRoomPrefab;
    public Color enemyGizmosColor;
    public int enemyRoomRate;
    [Space]
    public GameObject bossRoomPrefab;
    public Color bossGizmosColor;
    public int bossRoomRate;
}
