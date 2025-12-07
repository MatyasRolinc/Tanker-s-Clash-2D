using UnityEngine;
using TMPro;

public class UpgradeMenuUIScript : MonoBehaviour
{
    public PlayerStats playerStats; // nastav v Inspectoru

    public TextMeshProUGUI hpTMP;
    public TextMeshProUGUI speedTMP;
    public TextMeshProUGUI damageTMP;
    public TextMeshProUGUI reloadTMP;
    public TextMeshProUGUI shellSpeedTMP;

    public int healthCost = 50;
    public int healthAdd = 1;

    public int speedCost = 40;
    public float speedAdd = 0.5f;

    public int rotationCost = 30;
    public float rotationAdd = 10f;

    public int reloadCost = 60;
    public float reloadDelta = -0.05f;
    public int shellSpeedCost = 40;
    public float shellSpeedAdd = 1f;

    void Start() { RefreshUI(); }

    public void RefreshUI()
    {
        if (playerStats != null)
        {
            if (hpTMP != null) hpTMP.text = playerStats.maxHealth.ToString();
            if (speedTMP != null) speedTMP.text = playerStats.moveSpeed.ToString("0.##");
            if (damageTMP != null) damageTMP.text = "—";
            if (reloadTMP != null) reloadTMP.text = $"{playerStats.reloadTime:0.00}s";
            if (shellSpeedTMP != null) shellSpeedTMP.text = playerStats.shellSpeed.ToString("0.##");
        }
    }

    // debugované tlačítko: vypíše důvod pokud nic neproběhne
    public void BuyHealthUpgrade()
    {
        Debug.Log("BuyHealthUpgrade pressed");
        if (playerStats == null) { Debug.LogWarning("BuyHealthUpgrade: playerStats is null"); return; }
        if (!playerStats.SpendMoney(healthCost)) { Debug.Log("BuyHealthUpgrade: not enough money"); return; }

        playerStats.maxHealth += healthAdd;
        playerStats.health = Mathf.Clamp(playerStats.health + healthAdd, 0, playerStats.maxHealth);
        playerStats.UpdateUI();
        RefreshUI();
        Debug.Log($"Bought health +{healthAdd}. Money left: {playerStats.money}");
    }

    public void BuySpeedUpgrade()
    {
        Debug.Log("BuySpeedUpgrade pressed");
        if (playerStats == null) { Debug.LogWarning("BuySpeedUpgrade: playerStats is null"); return; }
        if (!playerStats.SpendMoney(speedCost)) { Debug.Log("BuySpeedUpgrade: not enough money"); return; }

        playerStats.moveSpeed += speedAdd;
        playerStats.UpdateUI();
        RefreshUI();
        Debug.Log($"Bought speed +{speedAdd}. Money left: {playerStats.money}");
    }

    public void BuyReloadUpgrade()
    {
        Debug.Log("BuyReloadUpgrade pressed");
        if (playerStats == null) { Debug.LogWarning("BuyReloadUpgrade: playerStats is null"); return; }
        if (!playerStats.SpendMoney(reloadCost)) { Debug.Log("BuyReloadUpgrade: not enough money"); return; }

        playerStats.reloadTime = Mathf.Max(0.01f, playerStats.reloadTime + reloadDelta);
        playerStats.UpdateUI();
        RefreshUI();
        Debug.Log($"Bought reload {reloadDelta}. Money left: {playerStats.money}");
    }

    public void BuyShellSpeedUpgrade()
    {
        Debug.Log("BuyShellSpeedUpgrade pressed");
        if (playerStats == null) { Debug.LogWarning("BuyShellSpeedUpgrade: playerStats is null"); return; }
        if (!playerStats.SpendMoney(shellSpeedCost)) { Debug.Log("BuyShellSpeedUpgrade: not enough money"); return; }

        playerStats.shellSpeed += shellSpeedAdd;
        playerStats.UpdateUI();
        RefreshUI();
        Debug.Log($"Bought shellSpeed +{shellSpeedAdd}. Money left: {playerStats.money}");
    }
}
