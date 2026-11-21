using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum RoomType
{
    Empty,
    Loot,
    Enemy,
    Boss
}

public class RoomBehavior : MonoBehaviour
{
    [SerializeField] private SO_DungeonValues values;
    [SerializeField] private GameObject[] m_walls; // [0] Up(+z); [1] Down(-z); [2] Right(+x); [3] Left(-x)
    [SerializeField] private GameObject[] m_doors;

    [SerializeField] public RoomType m_Type;

    public void UpdateRoom(bool[] _status, bool _firstRoom = false)
    {
        for (int i = 0; i < _status.Length; i++)
        {
            m_doors[i].SetActive(_status[i]);
            m_walls[i].SetActive(!_status[i]);
        }
        if (_firstRoom)
        {
            m_doors[0].SetActive(true);
            m_walls[0].SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        var certerOffset = new Vector3(1.5f, 2f, 1.5f) + transform.position;
        switch (m_Type)
        {
            case RoomType.Empty:
                Gizmos.color = values.emptyGizmosColor;
                break;

            case RoomType.Loot:
                Gizmos.color = values.lootGizmosColor;
                break;

            case RoomType.Enemy:
                Gizmos.color = values.enemyGizmosColor;
                break;

            case RoomType.Boss:
                Gizmos.color = values.bossGizmosColor;
                break;
        }
        Gizmos.DrawWireCube(certerOffset, new(17f, 4f, 17f));
        Gizmos.DrawWireCube(certerOffset, new(6f, 4f, 6f));
    }
}
