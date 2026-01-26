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
       if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Tento objekt teď přežije změnu scény
    }
    else
    {
        Destroy(gameObject); // Pokud už jeden existuje, tento nový smažeme
        return;
    }
    }

    private void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        initialEnemyCount = enemiesRemaining;
        Debug.Log($"LevelManager: Enemies counted at start = {initialEnemyCount}");

        
    }

    private void OnEnable()
{
    // Přihlásíme se k odběru události načtení scény
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    // Odhlásíme se (prevence chyb)
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

// Tato metoda se spustí pokaždé, když se načte jakákoliv scéna
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Resetujeme flag a spočítáme nepřátele v nové scéně
    levelCompletedFlag = false;
    enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
    initialEnemyCount = enemiesRemaining;

    Debug.Log($"Scéna {scene.name} načtena. Nepřátel nalezeno: {enemiesRemaining}");
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
        levelCompletedFlag = true; // Zabráníme vícenásobnému spuštění

        // Uložíme index pro příště do LevelManager.nextLevelIndex
        nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
            nextLevelIndex = 0; // zpět do menu/upgrade pokud není další level

        Debug.Log("Vše mrtvé, jdu do UpgradeScene");
        SceneManager.LoadScene("UpgradeScene"); // UJISTI SE, ŽE SE TAK SCÉNA JMENUJE!
    }

    

    // Tuto metodu nastav na tlačítko "POKRAČOVAT" v Upgrade Menu
    public void LoadNextLevel()
    {
        // Použijeme `nextLevelIndex` nastavený při dokončení levelu
        int indexToLoad = nextLevelIndex;

        // Kontrola, zda scéna existuje
        if (indexToLoad < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(indexToLoad);
        }
        else
        {
            Debug.Log("Konec hry!");
            SceneManager.LoadScene(0); // Zpět do menu/upgrade
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