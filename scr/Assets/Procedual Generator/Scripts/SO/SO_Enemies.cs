using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Mage,
    Knight,
    Boss
}

[CreateAssetMenu(menuName = "Create Enemy")]
public class SO_Enemies : ScriptableObject
{
    public int maxHealth;
    public int hp;
    public int dmg;
    
    public EnemyType enemyType;
    public GameObject prefab;
}
