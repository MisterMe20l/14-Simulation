using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "New Room")]
public class SO_Rooms : ScriptableObject
{
    public GameObject RoomPrefab;
    public string RoomName;
    public string RoomFolderName;
    public List<GameObject> Decorations = new();
    private void OnDestroy()
    {
         
        RoomPrefab = null;
    }
}
