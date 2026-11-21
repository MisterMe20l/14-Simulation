using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public List<SO_Enemies> enemies;
    [SerializeField] private bool bossRoom;
    public List<GameObject> EnemiesInRoom;

    private float minSpawnDist = 3f;

    public void InstantiateEnemies()
    {
        EnemiesInRoom = new();
        var spawnPos = transform.position + new Vector3(1.5f, 0f, 1.5f); 
        var fixedSpawnPos = spawnPos; //To compare
        if (!bossRoom)
        {
            var posLog = new List<Vector3>();
            for (int i = 0; i < enemies.Count; i++) //Instantiate all enemies in the list
            {
                if (i != 0)
                {
                    while (true) //Don't overlap spawn
                    {
                        var rndPos = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                        spawnPos = fixedSpawnPos + rndPos;
                        if (i == 1 && Vector3.Distance(spawnPos, posLog[i - 1]) > minSpawnDist) break;
                        else if (Vector3.Distance(spawnPos, posLog[i - 1]) > minSpawnDist && 
                            Vector3.Distance(spawnPos, posLog[i - 2]) > minSpawnDist) break;
                    } 
                }
                posLog.Add(spawnPos);

                EnemiesInRoom.Add(GameObject.Instantiate(enemies[i].prefab, spawnPos, Quaternion.identity, transform));
            }
        }
        else
        {
            EnemiesInRoom.Add(GameObject.Instantiate(enemies[0].prefab, spawnPos, Quaternion.identity, transform));
        }
    }
}
