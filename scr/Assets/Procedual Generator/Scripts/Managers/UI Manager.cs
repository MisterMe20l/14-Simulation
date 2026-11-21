using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timeCounter;
    [SerializeField] private TextMeshProUGUI m_playerStats;
    [SerializeField] private TextMeshProUGUI m_playerHealth;
    [SerializeField] private SO_Time time;
    [SerializeField] private SO_Player player;

    private string day;
    private string hour;
    private string minute;

    private string hp;
    private string kills;
    private string loot;
    private string dmg;

    private void Update()
    {
        UpdateTimeCounter();
        UpdatePlayerStats();
    }

    private void UpdateTimeCounter()
    {
        day = (time.Day < 10) ? $"0{time.Day}" : $"{time.Day}";
        hour = (time.Hour < 10) ? $"0{time.Hour}" : $"{time.Hour}";
        minute = (time.Minute < 10) ? $"0{time.Minute}" : $"{time.Minute}";
        m_timeCounter.SetText($" {day}\n {hour}:{minute}");
    }

    private void UpdatePlayerStats()
    {
        hp = ( (player.hp < 100) ? ((player.hp < 10) ? $"00{player.hp}" : $"0{player.hp}") : $"{player.hp}" ) + $" / {player.maxHealth}";
        hp = (player.hp <= 0) ? "000" : hp;
        dmg = $"{player.dmg}";
        kills = (player.enemiesKilled < 10) ? $"0{player.enemiesKilled}" : $"{player.enemiesKilled}";
        loot = (player.lootedRooms < 10) ? $"0{player.lootedRooms}" : $"{player.lootedRooms}";
        m_playerStats.SetText($" {dmg}\n {kills}\n {loot}");
        m_playerHealth.SetText($"HP: {hp}");
    }
}
