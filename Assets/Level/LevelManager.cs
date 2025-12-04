using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // centralní udělení peněz — přednostně použije killer (projektil / hráč), jinak najde hráče podle tagu "Player"
    public void AwardMoney(int amount, GameObject killer = null)
    {
        if (amount == 0) return;

        // pokud je předán killer, zkus najít BodyScript na něm nebo parentu
        if (killer != null)
        {
            var bs = killer.GetComponent<BodyScript>() ?? killer.GetComponentInParent<BodyScript>();
            if (bs != null)
            {
                bs.AddMoney(amount);
                return;
            }
        }

        // fallback: najdi hráče podle tagu "Player"
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerBs = player.GetComponent<BodyScript>();
            if (playerBs != null)
            {
                playerBs.AddMoney(amount);
                return;
            }
        }

        Debug.LogWarning("LevelManager: AwardMoney - nelze najít BodyScript pro připsání peněz.");
    }
}
