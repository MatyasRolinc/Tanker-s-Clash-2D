using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Nastavení Levelu")]
    // ZMĚNĚNO: Teď je tu velké "E", aby to sedělo s tvým Unity nastavením
    public string enemyTag = "Enemy"; 
    public string upgradeSceneName = "UpgradeScene";
    public GameObject pauseMenuPanel;
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
 // Sem v Inspektoru přetáhneš ten Panel
    private bool isPaused = false;
    
    [Header("Stav Levelu")]
    public int enemiesRemaining = 0;
    private bool levelCompletedFlag = false;
    private int lastLevelIndex = 0; // Pomocná proměnná pro sledování postupu

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Debug.Log("ESC byl stisknut!");
                PauseGame();
            }
        }
    }

    
    
    // Metody přidané zpět: StartGame a QuitGame
    public void StartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        // Načte scénu, která je v Build Settings hned za Menu (index 1)
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        // Pokud se vracíme do menu, vždy se ujistíme, že hra běží normálně
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
    if (pauseMenuPanel == null) return; // Pojistka, kdybychom panel zapomněli přiřadit

    pauseMenuPanel.SetActive(true);
    Time.timeScale = 0f; // TADY ZASTAVÍME ČAS VE HŘE
    isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel == null) return;

        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // znovu spustíme čas
        isPaused = false;
    }
    public void QuitGame()
    {
        Debug.Log("Vypínám hru...");
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelCompletedFlag = false;

        // Pokud jsme v upgradu, nehledáme nepřátele
        if (scene.name == upgradeSceneName) return;
        PlayerStats ps = Object.FindFirstObjectByType<PlayerStats>();
        if (ps != null) 
        {
            ps.ResetHealth();
        }

        try 
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            enemiesRemaining = enemies.Length;
            Debug.Log($"Scéna {scene.name} načtena. Nepřátel: {enemiesRemaining}");
        }
        catch 
        {
            enemiesRemaining = 0;
        }

        GameObject foundPanel = GameObject.Find("PauseMenu");

        if (foundPanel != null)
        {
            pauseMenuPanel = foundPanel;
            pauseMenuPanel.SetActive(false);
            Debug.Log("PauseMenu úspěšně nalezeno a skryto.");
        }
        else
        {
            Debug.LogWarning("LevelManager pořád nemůže najít PauseMenu. Zkontroluj jméno!");
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void EnemyKilled()
    {
        if (levelCompletedFlag) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        enemiesRemaining = Mathf.Max(0, enemies.Length - 1);

        if (enemiesRemaining <= 0)
        {
            LevelCompleted();
        }
    }

    void LevelCompleted()
    {
        if (levelCompletedFlag) return;
        levelCompletedFlag = true;

        // Uložíme si index levelu, který jsme právě dohráli
        lastLevelIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log($"Level {lastLevelIndex} hotov. Jdu do upgradu.");
        SceneManager.LoadScene(upgradeSceneName);
    }

    // TATO METODA JE OPRAVENÁ PROTI CYKLENÍ
    public void LoadNextLevel()
    {
        int nextLevel = lastLevelIndex + 1;

        // Kontrola: Pokud je další scéna v pořadí UpgradeMenu, přeskoč ji na další index
        string nextSceneName = GetSceneNameFromIndex(nextLevel);
        if (nextSceneName == upgradeSceneName)
        {
            nextLevel++;
        }

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Načítám scénu s indexem: {nextLevel}");
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.Log("Konec hry, vracím do menu.");
            SceneManager.LoadScene(0); 
        }
    }

    // Pomocná funkce pro zjištění jména scény podle indexu
    string GetSceneNameFromIndex(int index)
    {
        if (index >= SceneManager.sceneCountInBuildSettings) return "";
    
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        int slash = path.LastIndexOf('/');
        int dot = path.LastIndexOf('.');
        return path.Substring(slash + 1, dot - slash - 1);
    }

    public void AwardMoney(int amount)
    {
        if (amount <= 0) return;
        PlayerStats ps = Object.FindFirstObjectByType<PlayerStats>();
        if (ps != null) ps.AddMoney(amount);
    }

    // Called when the player dies to return to the main menu scene named "MainMenu"
    public void ReturnToMainMenuScene()
    {
        // Ensure game is unpaused and timeScale reset
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Try to load scene named "MainMenu"; fallback to build index 0 if not found
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("MainMenu scene not found by name, loading build index 0 instead.");
            SceneManager.LoadScene(0);
        }
    }
}