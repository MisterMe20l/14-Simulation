using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class DungeonGeneratorWindow : EditorWindow
{
    private static DungeonGeneratorWindow currentWindow;
    private static Vector2 windowSize = new Vector2(500, 595);

    private int startPoint = 0;
    private string valuesPath = "Assets/Procedual Generator/Scripts/Editor/Values.asset";

    private SO_DungeonValues values;

    [MenuItem("Tools/Dungeon Generator _#f")] //_#g (Shift + G)
    private static void InitWindow()
    {
        currentWindow = EditorWindow.GetWindow<DungeonGeneratorWindow>();
        currentWindow.titleContent = new GUIContent("Dungeon Generator");
        currentWindow.minSize = windowSize;
        currentWindow.maxSize = windowSize;
    }

    private void OnEnable()
    {
        values = AssetDatabase.LoadAssetAtPath<SO_DungeonValues>(valuesPath);
    }

    private void OnGUI()
    {
        var rPos = new Vector2(10, 10);
        var rSize = new Vector2(windowSize.x - 2 * rPos.x, 20);
        var rOffset = rSize.x / 3 - 5;

        var style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        //Draw Transform Fields:
        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Transform:", style);
        rPos.y += 20;
        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Size:");
        var tSize = EditorGUI.Vector2Field(new Rect(rPos.x + rOffset, rPos.y, rSize.x - rOffset, rSize.y), "", values.size);
        if (tSize != values.size) values.size = tSize;
        rPos.y += 25;

        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Offset:");
        var tOffset = EditorGUI.Vector2Field(new Rect(rPos.x + rOffset, rPos.y, rSize.x - rOffset, rSize.y), "", values.offset);
        if (tOffset != values.offset) values.offset = tOffset;
        rPos.y += 25;

        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Start Position:");
        var tStartPos = EditorGUI.Vector3Field(new Rect(rPos.x + rOffset, rPos.y, rSize.x - rOffset, rSize.y), "", values.startPos);
        if (tStartPos != values.startPos) values.startPos = tStartPos;
        rPos.y += 25;

        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Instantiate Enemies?");
        var instantiateEnemies = EditorGUI.Toggle(new Rect(rPos.x + (rSize.x / 4) + 10, rPos.y, rSize.y, rSize.y),
            "", values.instantiate);
        if (instantiateEnemies != values.instantiate) values.instantiate = instantiateEnemies;

        GUI.Label(new Rect(rPos.x + (rSize.x / 3) + 10, rPos.y, rSize.x, rSize.y), "NavMesh:");
        var navMesh = EditorGUI.ObjectField(new Rect(rPos.x + (rSize.x / 2), rPos.y, rSize.x / 2, rSize.y),
            "", values.navMesh, typeof(NavMeshSurface), true);
        if (navMesh != values.navMesh) values.navMesh = (NavMeshSurface)navMesh;
        rPos.y += 30;

        //Draw Prefabs Fields && Gizmos Colors:
        GUI.Box(new Rect(rPos.x, rPos.y, rSize.x, 400), "");
        rPos.y += 5;
        rPos.x += 5;
        rSize.x -= 10;
        //Empty Room
        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Empty Room:", style);
        rPos.y += 20;
        var emptyPrefab = EditorGUI.ObjectField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Prefab:", values.emptyRoomPrefab, typeof(GameObject), true);
        if (emptyPrefab != values.emptyRoomPrefab) values.emptyRoomPrefab = (GameObject)emptyPrefab;
        rPos.y += 25;

        var emptyGColor = EditorGUI.ColorField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Outline", values.emptyGizmosColor);
        if (emptyGColor != values.emptyGizmosColor) values.emptyGizmosColor = emptyGColor;
        rPos.y += 25;

        var emptyRRate = EditorGUI.IntSlider(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Rate (%):", values.emptyRoomRate, 0, 100);
        rPos.y += 30;

        //Loot Room
        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Loot Room:", style);
        rPos.y += 20;
        var lootPrefab = EditorGUI.ObjectField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Prefab:", values.lootRoomPrefab, typeof(GameObject), true);
        if (lootPrefab != values.lootRoomPrefab) values.lootRoomPrefab = (GameObject)lootPrefab;
        rPos.y += 25;

        var lootGColor = EditorGUI.ColorField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Outline", values.lootGizmosColor);
        if (lootGColor != values.lootGizmosColor) values.lootGizmosColor = lootGColor;
        rPos.y += 25;

        var lootRRate = EditorGUI.IntSlider(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Rate (%):", values.lootRoomRate, 0, 100);
        rPos.y += 30;

        //Enemy Room
        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Enemy Room:", style);
        rPos.y += 20;
        var enemyPrefab = EditorGUI.ObjectField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Prefab:", values.enemyRoomPrefab, typeof(GameObject), true);
        if (enemyPrefab != values.enemyRoomPrefab) values.enemyRoomPrefab = (GameObject)enemyPrefab;
        rPos.y += 25;

        var enemyGColor = EditorGUI.ColorField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Outline", values.enemyGizmosColor);
        if (enemyGColor != values.enemyGizmosColor) values.enemyGizmosColor = enemyGColor;
        rPos.y += 25;

        var enemyRRate = EditorGUI.IntSlider(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Rate (%):", values.enemyRoomRate, 0, 100);
        rPos.y += 30;

        //Boss Room
        GUI.Label(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Boss Room:", style);
        rPos.y += 20;
        var bossPrefab = EditorGUI.ObjectField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Rate:", values.bossRoomPrefab, typeof(GameObject), true);
        if (bossPrefab != values.bossRoomPrefab) values.bossRoomPrefab = (GameObject)bossPrefab;
        rPos.y += 25;

        var bossGColor = EditorGUI.ColorField(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Outline", values.bossGizmosColor);
        if (bossGColor != values.bossGizmosColor) values.bossGizmosColor = bossGColor;
        rPos.y += 25;

        var bossRRate = EditorGUI.IntSlider(new Rect(rPos.x, rPos.y, rSize.x, rSize.y),
            "Rate (%):", values.bossRoomRate, 0, 100);
        rPos.y += 30;

        //Ajust Sliders:
        AjustRoomRates(emptyRRate, lootRRate, enemyRRate, bossRRate);

        //Draw Buttons:
        rPos.x -= 5;
        rSize.x += 10;
        if (GUI.Button(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Generate Dungeon"))
        {
            if (board == null)
            {
                Random.InitState(Random.Range(0, 99999));
                MazeGenerator();
                values.generated = true;
                values.navMesh.BuildNavMesh();
            }
        }
        rPos.y += 25;

        if (GUI.Button(new Rect(rPos.x, rPos.y, rSize.x, rSize.y), "Destroy Dungeon"))
        {
            DestroyDungeon();
            values.generated = false;
            values.navMesh.BuildNavMesh();
        }
        rPos.y += 30;
        //Debug.Log("Window height: " + rPos.y + " px");
    }

    private void AjustRoomRates(int _emptyRR, int _lootRR, int _enemyRR, int _bossRR)
    {
        var iValues = new int[] { _emptyRR, _lootRR, _enemyRR, _bossRR };
        var total = iValues[0] + iValues[1] + iValues[2] + iValues[3];
        var index = 0;
        var dif = 0;
        if (iValues[0] != values.emptyRoomRate)
        {
            index = 0;
            dif = values.emptyRoomRate - iValues[0];
            values.emptyRoomRate = iValues[0];
        }
        else if (iValues[1] != values.lootRoomRate)
        {
            index = 2;
            dif = values.lootRoomRate - iValues[1];
            values.lootRoomRate = iValues[1];
        }
        else if (iValues[2] != values.enemyRoomRate)
        {
            index = 2;
            dif = values.enemyRoomRate - iValues[2];
            values.enemyRoomRate = iValues[2];
        }
        else if (iValues[3] != values.bossRoomRate)
        {
            index = 2;
            dif = values.bossRoomRate - iValues[3];
            values.bossRoomRate = iValues[3];
        }

        var excess = total - 100;
        for (int i = 0; i < iValues.Length; i++)
        {
            if (i == index) continue; // Skip changed slider

            int adjustAmount = Mathf.RoundToInt((float)iValues[i] / (100 - iValues[index]) * excess);
            iValues[i] = Mathf.Clamp(iValues[i] - adjustAmount, 0, 100);
        }

        values.emptyRoomRate = iValues[0];
        values.lootRoomRate = iValues[1];
        values.enemyRoomRate = iValues[2];
        values.bossRoomRate = iValues[3];
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
        if (board != null)
        {
            for (int i = 0; i < values.rooms.Count; i++)
            {
                GameObject.DestroyImmediate(values.rooms[i]);
            }
            values.rooms = null;
            board = null;
        }
    }
}