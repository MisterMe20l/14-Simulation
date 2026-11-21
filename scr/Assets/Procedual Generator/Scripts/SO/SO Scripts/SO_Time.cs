using UnityEngine;

[CreateAssetMenu(menuName = "Time")]
public class SO_Time : ScriptableObject
{
    [SerializeField] public int Day;
    [SerializeField] public int Hour;
    [SerializeField] public int Minute;
}
