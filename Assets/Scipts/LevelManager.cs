using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Nastavení Levelu")]
    public string enemyTag = "Enemy"; 
    public string upgradeSceneName = "UpgradeScene";
    public bool isLastLevel = false;
    public GameObject pauseMenu;
    public GameObject deathScreen;
    public GameObject winScreen;
    private bool isPaused = false;
    public static LevelManager Instance;
    
    public int enemiesRemaining = 0;
    private bool levelCompletedFlag = false;
    private static int lastLevelIndex = 0;

    void Start()
    {
        levelCompletedFlag = false;

        if (SceneManager.GetActiveScene().name == upgradeSceneName) return;
        PlayerStats ps = Object.FindFirstObjectByType<PlayerStats>();
        if (ps != null) 
        {
            ps.ResetHealth();
        }

        try 
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            enemiesRemaining = enemies.Length;
            Debug.Log($"Scéna {SceneManager.GetActiveScene().name} načtena. Nepřátel: {enemiesRemaining}");
        }
        catch 
        {
            enemiesRemaining = 0;
        }

        Time.timeScale = 1f;
        isPaused = false;
    }
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu == null)
            {
                pauseMenu = GameObject.Find("PauseMenu");
            }
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                if (deathScreen != null && deathScreen.activeSelf) return;
                PauseGame();
            }
        }
    }

    
    
    public void StartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        // Načte scénu, která je v Build Settings hned za Menu (index 1)
        SceneManager.LoadScene(1);
    }

    public void Die()
    {
    if (deathScreen != null)
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f; 
        Debug.Log("Hráč zemřel, DeathScreen aktivováno.");
    }
    else
    {
        Debug.LogError("Chyba: V Inspektoru chybí přetažený DeathScreen!");
    }
    }

    public void ReturnToMainMenu()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        isPaused = false;
    }
    public void QuitGame()
    {
        Debug.Log("Vypínám hru...");
        Application.Quit();
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

        if (isLastLevel)
        {
            if (winScreen != null)
            {
                winScreen.SetActive(true);
            }
            Time.timeScale = 0f;

            if (PlayerStats.instance != null)
            {
                Destroy(PlayerStats.instance.gameObject);
            }
        }
        else
        {
            SceneManager.LoadScene(upgradeSceneName);
        }
    }
    public void LoadNextLevel()
    {
        int nextLevel = lastLevelIndex + 1;

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
    public void ReturnToMainMenuScene()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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