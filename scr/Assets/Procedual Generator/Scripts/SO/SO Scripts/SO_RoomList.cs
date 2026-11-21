using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room List")]
public class SO_RoomList : ScriptableObject
{
    public SO_Rooms currentRoom;
    public string FolderPath = "Assets/Room List";
    public List<SO_Rooms> AllRoomsList = new ();
}
