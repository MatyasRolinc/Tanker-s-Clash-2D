using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public int rewardAmount = 50000; // Nastavíš v Inspectoru pro každého nepřítele jinak

    // Tuto metodu zavolej ve skriptu nepřítele, když mu klesnou životy na 0
    public void GiveReward()
    {
        if (PlayerStats.instance != null)
        {
            // Přičteme peníze přímo do globálních statistik
            PlayerStats.instance.AddMoney(rewardAmount);
            Debug.Log($"Nepřítel poražen! Přidáno {rewardAmount} peněz. Celkem: {PlayerStats.instance.money}");
        }
        else
        {
            Debug.LogWarning("EnemyReward: Nelze připsat odměnu, PlayerStats.instance chybí!");
        }
    }
}