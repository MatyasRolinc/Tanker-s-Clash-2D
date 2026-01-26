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
    public int reloadCost = 60;
    public int shellSpeedCost = 40;
    public int damageCost = 70;

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

    public void RefreshUI()
{
    // Toto zajistí, že vždy bereme data z té "nesmrtelné" instance
    playerStats = PlayerStats.instance;

    if (playerStats == null) 
    {
        Debug.LogError("RefreshUI: Stále nemůžu najít PlayerStats.instance!");
        return;
    }

    // Teď vypíšeme hodnoty - Všimni si Debug logu, ten nám řekne pravdu
    Debug.Log($"Vypisuji do UI: HP={playerStats.maxHealth}, Speed={playerStats.moveSpeed}");

    if (hpTMP != null) hpTMP.text = playerStats.maxHealth.ToString();
    if (speedTMP != null) speedTMP.text = playerStats.moveSpeed.ToString("F1");
    if (reloadTMP != null) reloadTMP.text = playerStats.reloadTime.ToString("F2");
    if (shellSpeedTMP != null) shellSpeedTMP.text = playerStats.shellSpeed.ToString("F1");

    playerStats.UpdateUI();
}

    // --- METODY PRO TLAČÍTKA ---

    public void BuyHealthUpgrade()
    {   RefreshUI();    
        Debug.Log("KLIKNUTO na Health Upgrade");
        Debug.Log("BuyHealthUpgrade called. playerStats present=" + (playerStats != null) + ", money=" + (playerStats!=null?playerStats.money.ToString():"null"));
        if (CheckStats() && playerStats.SpendMoney(healthCost))
        {
            RefreshUI();
            playerStats.maxHealth += 1;
            playerStats.health += 1; // Přidá život i do aktuálního zdraví
            Debug.Log("<color=cyan>Upgrade KOUPEN! Max HP: </color>" + playerStats.maxHealth);
            
        }
        RefreshUI();
    }

    public void BuySpeedUpgrade()
    {   RefreshUI();
        Debug.Log("KLIKNUTO na Speed Upgrade");
        Debug.Log("BuySpeedUpgrade called. playerStats present=" + (playerStats != null) + ", money=" + (playerStats!=null?playerStats.money.ToString():"null"));
        if (CheckStats() && playerStats.SpendMoney(speedCost))
        {
            playerStats.moveSpeed += 0.5f;
            Debug.Log("<color=cyan>Upgrade KOUPEN! Rychlost: </color>" + playerStats.moveSpeed);
            
        }
        RefreshUI();
    }

    public void BuyReloadUpgrade()
    {   RefreshUI();
        Debug.Log("KLIKNUTO na Reload Upgrade");
        Debug.Log("BuyReloadUpgrade called. playerStats present=" + (playerStats != null) + ", money=" + (playerStats!=null?playerStats.money.ToString():"null"));
        if (CheckStats() && playerStats.SpendMoney(reloadCost))
        {
            // Snížení času (přebíjíš rychleji), minimum je 0.1s
            playerStats.reloadTime = Mathf.Max(0.1f, playerStats.reloadTime - 0.05f);
            Debug.Log("<color=cyan>Upgrade KOUPEN! Reload Time: </color>" + playerStats.reloadTime);
            
        }
        RefreshUI();
    }

    public void BuyShellSpeedUpgrade()
    {   RefreshUI();
        Debug.Log("KLIKNUTO na Shell Speed Upgrade");
        Debug.Log("BuyShellSpeedUpgrade called. playerStats present=" + (playerStats != null) + ", money=" + (playerStats!=null?playerStats.money.ToString():"null"));
        if (CheckStats() && playerStats.SpendMoney(shellSpeedCost))
        {
            playerStats.shellSpeed += 1.0f;
            Debug.Log("<color=cyan>Upgrade KOUPEN! Rychlost střely: </color>" + playerStats.shellSpeed);
            
        }
        RefreshUI();
    }

    // TLAČÍTKO PRO DALŠÍ LEVEL
    public void ClickNextLevel()
    {
        Debug.Log("Tlačítko Next Level stisknuto.");

    // Pokusíme se najít LevelManager v aktuální scéně
    // Pokud ho tam máš jako Singleton (LevelManager.Instance)
    if (LevelManager.Instance != null)
    {
        // Tady voláme metodu, kterou máš v LevelManageru
        // Pokud se jmenuje jinak (např. LoadNextScene), přejmenuj to zde
        LevelManager.Instance.LoadNextLevel(); 
    }
    else
    {
        // Pokud LevelManager není Singleton, zkusíme ho najít přes Find
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