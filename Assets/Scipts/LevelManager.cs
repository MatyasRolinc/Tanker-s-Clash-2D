using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // PŘIDÁNO: Nutné pro přepínání scén

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject upgradeMenuCanvas; 
    public GameObject hudCanvas;    

    public static int nextLevelIndex = 1;     
    int enemiesRemaining = 0;
    int initialEnemyCount = 0;
    bool levelCompletedFlag = false;

    private void Awake()
    {
        // POKUD chceš, aby peníze a stav levelu přežily víc scén, 
        // přidej sem DontDestroyOnLoad(gameObject), ale pak musíš 
        // hlídat, aby se Instance neduplikovala.
        Instance = this;
    }

    private void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        initialEnemyCount = enemiesRemaining;
        Debug.Log($"LevelManager: Enemies counted at start = {initialEnemyCount}");

        
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
       Debug.Log("Level dokončen!");
    
    // Načte scénu podle indexu v Build Settings
    // 0 je tvoje UpgradeMenu
    SceneManager.LoadScene(0);
    }

    // Tuto metodu nastav na tlačítko "POKRAČOVAT" v Upgrade Menu
    public void LoadNextLevel()
    {
       // 1. Zjistíme index aktuálně otevřené scény
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 2. Vypočítáme index další scény
        int nextSceneIndex = currentSceneIndex + 1;

        // 3. Zkontrolujeme, jestli další scéna v Build Settings vůbec existuje
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Jsi na konci! Žádný další level v Build Settings není.");
            // Tady můžeš třeba načíst znovu menu (index 0)
            // SceneManager.LoadScene(0);
        }
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

        Debug.LogWarning("LevelManager: AwardMoney - PlayerStats not found.");
    }
}