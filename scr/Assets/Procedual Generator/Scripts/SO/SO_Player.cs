using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class SO_Player : ScriptableObject
{
    public int startMaxHealth;
    public int startDmg;
    public int maxHealth;
    public int hp;
    public int dmg;
    public bool tookDmg;

    public int enemiesKilled;
    public int lootedRooms;
}
