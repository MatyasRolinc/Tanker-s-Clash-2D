using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Potřebujeme pro přepínání levelů

public class UpgradeMenuUIScript : MonoBehaviour
{
    private PlayerStats playerStats;

    [Header("UI Texty")]
    public TextMeshProUGUI hpTMP;
    public TextMeshProUGUI speedTMP;
    public TextMeshProUGUI reloadTMP;
    public TextMeshProUGUI shellSpeedTMP;
    public TextMeshProUGUI damageTMP; // Přidáno pro damage

    [Header("Ceny")]
    public int healthCost = 50;
    public int speedCost = 40;
    public int reloadCost = 50;
    public int shellSpeedCost = 40;
    public int damageCost = 100;

    void Start() 
    { 
        if (PlayerStats.instance != null)
        {
            playerStats = PlayerStats.instance;
            Debug.Log("<color=green>UpgradeMenu: PlayerStats nalezen!</color>");
            RefreshUI();
        }
        else
        {
            Debug.LogError("<color=red>UpgradeMenu: PlayerStats NENALEZEN! Spusť hru z Levelu 1!</color>");
        }
    }

    void OnEnable()
{
    // Zkusíme najít instanci hned několikrát
    Invoke(nameof(LateRefresh), 0.1f); // Počkáme 0.1 sekundy, než se vše usadí
}

void LateRefresh()
{
    RefreshUI();
}

    public void RefreshUI()
{
    playerStats = PlayerStats.instance;

    if (playerStats == null) 
    {
        Debug.LogError("RefreshUI: Stále nemůžu najít PlayerStats.instance!");
        return;
    }

    Debug.Log($"Vypisuji do UI: HP={playerStats.maxHealth}, Speed={playerStats.moveSpeed}");

    if (hpTMP != null) hpTMP.text = playerStats.maxHealth.ToString();
    if (speedTMP != null) speedTMP.text = playerStats.moveSpeed.ToString("F1");
    if (reloadTMP != null) reloadTMP.text = playerStats.reloadTime.ToString("F2");
    if (shellSpeedTMP != null) shellSpeedTMP.text = playerStats.shellSpeed.ToString("F1");
    if (damageTMP != null) damageTMP.text = playerStats.damage.ToString();

    playerStats.UpdateUI();
}

    // --- METODY PRO TLAČÍTKA ---

    public void BuyHealthUpgrade()
    {   RefreshUI();    
        if (CheckStats())
        {
            playerStats.UpgradeMaxHealth(healthCost);
        }
        RefreshUI();
    }

    public void BuySpeedUpgrade()
    {   RefreshUI();
        if (CheckStats())
        {
            playerStats.UpgradeMoveSpeed(speedCost);
        }
        RefreshUI();
    }

    public void BuyReloadUpgrade()
    {   RefreshUI();
        {
            playerStats.UpgradeReloadTime(reloadCost);
        }
        RefreshUI();
    }

    public void BuyShellSpeedUpgrade()
    {   RefreshUI();
        if (CheckStats() && playerStats.SpendMoney(shellSpeedCost))
        {
            playerStats.shellSpeed += 1.0f;
        }
        RefreshUI();
    }

    public void BuyDamageUpgrade()
    {
        RefreshUI();
        if (CheckStats())
        {
            playerStats.UpgradeDamage(damageCost);
        }
        RefreshUI();
    }

    // TLAČÍTKO PRO DALŠÍ LEVEL
    public void ClickNextLevel()
    {
        Debug.Log("Tlačítko Next Level stisknuto.");
    if (LevelManager.Instance != null)
    {
        LevelManager.Instance.LoadNextLevel(); 
    }
    else
    {
        LevelManager lm = FindFirstObjectByType<LevelManager>();
        if (lm != null)
        {
            lm.LoadNextLevel();
        }
    }
    }
    // Pomocná metoda, aby se kód neopakoval
    private bool CheckStats()
    {
        if (playerStats == null) playerStats = PlayerStats.instance;
    return playerStats != null;
    }
}