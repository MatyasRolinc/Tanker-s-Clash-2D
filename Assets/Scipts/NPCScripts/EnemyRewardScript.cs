using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public int rewardAmount = 0;

   
    public void GiveReward()
    {
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.AddMoney(rewardAmount);
            Debug.Log($"Nepřítel poražen! Přidáno {rewardAmount} peněz. Celkem: {PlayerStats.instance.money}");
        }
        else
        {
            Debug.LogWarning("EnemyReward: Nelze připsat odměnu, PlayerStats.instance chybí!");
        }
    }
}