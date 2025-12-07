using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject upgradeMenuCanvas; // přetáhni Canvas_UpgradeMenu
    public GameObject hudCanvas;         // HUD ve hře (pokud ho chceš skrývat)

    int enemiesRemaining = 0;
    int initialEnemyCount = 0;
    bool levelCompletedFlag = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        initialEnemyCount = enemiesRemaining;
        Debug.Log($"LevelManager: Enemies counted at start = {initialEnemyCount}");

        if (upgradeMenuCanvas != null) upgradeMenuCanvas.SetActive(false);
    }

    public void EnemyKilled()
    {
        if (levelCompletedFlag) return;

        enemiesRemaining--;
        Debug.Log($"LevelManager: Enemy killed. Remaining = {Mathf.Max(0, enemiesRemaining)}");

        if (enemiesRemaining <= 0)
            LevelCompleted();
    }

    void LevelCompleted()
    {
        if (levelCompletedFlag) return;
        levelCompletedFlag = true;

        Debug.Log($"LevelManager: LevelCompleted called. Initial enemies = {initialEnemyCount}, Remaining = {Mathf.Max(0, enemiesRemaining)}");

        Time.timeScale = 0f;                 // pauza hry

        if (upgradeMenuCanvas != null)
            upgradeMenuCanvas.SetActive(true);   // ukaž menu

        if (hudCanvas != null)
            hudCanvas.SetActive(false);

        // odstraněno: deaktivace root GameObjectů — nic jiného se nemění
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        if (upgradeMenuCanvas != null) upgradeMenuCanvas.SetActive(false);
        if (hudCanvas != null) hudCanvas.SetActive(true);

        levelCompletedFlag = false;

        // (Zde načti další scénu nebo restart pokud je třeba)
    }

    public void AwardMoney(int amount, GameObject source = null)
    {
        if (amount == 0) return;

        PlayerStats ps = null;

        if (source != null)
            ps = source.GetComponent<PlayerStats>() ?? source.GetComponentInParent<PlayerStats>();

        if (ps == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                ps = playerObj.GetComponent<PlayerStats>();
        }

        if (ps != null)
        {
            ps.AddMoney(amount);
            return;
        }

        Debug.LogWarning("LevelManager: AwardMoney - PlayerStats not found to add money.");
    }
}