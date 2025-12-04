using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public int rewardAmount = 10;

    // Zavolej tuto metodu z enemy skriptu těsně před zničením nepřítele.
    public void GiveReward(GameObject killer = null)
    {
        // pokud existuje LevelManager, použij ho
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.AwardMoney(rewardAmount, killer);
            return;
        }

        // fallback: původní logika (pokud LevelManager není přítomen)
        if (killer != null)
        {
            var bs = killer.GetComponent<BodyScript>() ?? killer.GetComponentInParent<BodyScript>();
            if (bs != null)
            {
                bs.AddMoney(rewardAmount);
                return;
            }
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerBs = player.GetComponent<BodyScript>();
            if (playerBs != null)
                playerBs.AddMoney(rewardAmount);
        }
    }
}