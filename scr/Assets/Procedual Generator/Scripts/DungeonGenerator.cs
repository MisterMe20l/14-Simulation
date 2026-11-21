using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] SO_DungeonValues values;
    [SerializeField] SO_Time clock;
    private int startPoint = 0;
    private bool midnight;
    private NavMeshSurface navMesh;

    private void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
        SetNewDungeon();
    }

    private void Update()
    {
        if (!midnight && clock.Hour == 1)
        {
            midnight = true;
            SetNewDungeon();
        }
        else if (clock.Hour != 1)
        {
            midnight = false;
        }
    }

    private void SetNewDungeon()
    {
        if (board != null) DestroyDungeon();
        MazeGenerator();
        navMesh.BuildNavMesh();
    }

    public class Cell
    {
        public bool Visited = false;
        public bool[] Status = new bool[4];
    }

    private List<Cell> board;

    private void GenerateDungeon()
    {
        values.rooms = new();

        for (int i = 0; i < values.size.x; i++)
        {
            for (int j = 0; j < values.size.y; j++)
            {
                var currentCell = board[Mathf.FloorToInt(i + j * values.size.x)];
                if (currentCell.Visited)
                {
                    var roomPos = new Vector3(i * values.offset.x, 0, -j * values.offset.y);
                    var rndRoomPrefab = GetRandomRoom();
                    var newRoom = Instantiate(rndRoomPrefab, values.startPos + roomPos, Quaternion.identity);

                    var newRoomBehavior = newRoom.GetComponent<RoomBehavior>();
                    newRoomBehavior.UpdateRoom(board[Mathf.FloorToInt(i + j * values.size.x)].Status);
                    if (values.instantiate && (newRoomBehavior.m_Type == RoomType.Enemy || newRoomBehavior.m_Type == RoomType.Boss))
                    {
                        newRoom.GetComponent<EnemyManager>().InstantiateEnemies();
                    }

                    newRoom.name += " " + i + "-" + j;
                    values.rooms.Add(newRoom);
                }
            }
        }
        values.rooms[0].GetComponent<RoomBehavior>().UpdateRoom(board[Mathf.FloorToInt(startPoint)].Status, true);
        values.lastRoomPos = values.rooms[values.rooms.Count - 1].transform.position + new Vector3(1.5f, 0f, 1.5f);
    }
    private GameObject GetRandomRoom()
    {
        GameObject nextRoom = null;
        var rndNum = Random.Range(0, 100);

        var emptyRR = new int[2] { 0, values.emptyRoomRate };
        var lootRR = new int[2] { emptyRR[1], emptyRR[1] + values.lootRoomRate };
        var enemyRR = new int[2] { lootRR[1], lootRR[1] + values.enemyRoomRate };
        var bossRR = new int[2] { enemyRR[1], enemyRR[1] + values.bossRoomRate };
        if (rndNum >= emptyRR[0] && rndNum < emptyRR[1])
        {
            nextRoom = values.emptyRoomPrefab;
        }
        else if (rndNum >= lootRR[0] && rndNum < lootRR[1])
        {
            nextRoom = values.lootRoomPrefab;
        }
        else if (rndNum >= enemyRR[0] && rndNum < enemyRR[1])
        {
            nextRoom = values.enemyRoomPrefab;
        }
        else if (rndNum >= bossRR[0] && rndNum < bossRR[1])
        {
            nextRoom = values.bossRoomPrefab;
        }
        return nextRoom;
    }

    public void MazeGenerator()
    {
        board = new();

        for (int i = 0; i < values.size.x; i++)
        {
            for (int j = 0; j < values.size.y; j++)
            {
                board.Add(new());
            }
        }

        var currentCell = startPoint;

        var path = new Stack<int>();
        values.roomIndexPath = new();

        var k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].Visited = true;

            if (currentCell == board.Count - 1) break;

            //Check cell's neighbors
            var neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);
                values.roomIndexPath.Add(currentCell);
                var rndNewCell = neighbors[Random.Range(0, neighbors.Count)];

                if (rndNewCell > currentCell) //down or right
                {
                    if (rndNewCell - 1 == currentCell)
                    {
                        board[currentCell].Status[2] = true;
                        currentCell = rndNewCell;
                        board[currentCell].Status[3] = true;
                    }
                    else
                    {
                        board[currentCell].Status[1] = true;
                        currentCell = rndNewCell;
                        board[currentCell].Status[0] = true;
                    }
                }
                else //up or left
                {
                    if (rndNewCell + 1 == currentCell)
                    {
                        board[currentCell].Status[3] = true;
                        currentCell = rndNewCell;
                        board[currentCell].Status[2] = true;
                    }
                    else
                    {
                        board[currentCell].Status[0] = true;
                        currentCell = rndNewCell;
                        board[currentCell].Status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    private List<int> CheckNeighbors(int _cell)
    {
        var neighbors = new List<int>();

        //Check up neighbor
        var upNeighborPos = Mathf.FloorToInt(_cell - values.size.x);
        if (_cell - values.size.x >= 0 && !board[upNeighborPos].Visited)
        {
            neighbors.Add(upNeighborPos);
        }

        //Check down neighbor
        var downNeighborPos = Mathf.FloorToInt(_cell + values.size.x);
        if (_cell + values.size.x < board.Count && !board[downNeighborPos].Visited)
        {
            neighbors.Add(downNeighborPos);
        }

        //Check right neighbor
        var rightNeighborPos = Mathf.FloorToInt(_cell + 1);
        if ((_cell + 1) % values.size.x != 0 && !board[rightNeighborPos].Visited)
        {
            neighbors.Add(rightNeighborPos);
        }

        //Check left neighbor
        var leftNeighborPos = Mathf.FloorToInt(_cell - 1);
        if (_cell % values.size.x != 0 && !board[leftNeighborPos].Visited)
        {
            neighbors.Add(leftNeighborPos);
        }

        return neighbors;
    }

    private void DestroyDungeon()
    {
        for (int i = 0; i < values.rooms.Count; i++)
        {
            GameObject.DestroyImmediate(values.rooms[i]);
        }
        values.rooms = null;
        board = null;
    }
}