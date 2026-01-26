using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Statistiky")]
    public int maxHealth = 5;
    public int health = 5;
    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;
    public int money = 5000;
    public float reloadTime = 0.75f;
    public float shellSpeed = 10f;

    [HideInInspector] public RectTransform healthBar;
    [HideInInspector] public TextMeshProUGUI moneyText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (health <= 0) health = maxHealth;
    }

    // --- LOGIKA FINANCÍ ---
    public bool SpendMoney(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            UpdateUI();
            return true; // Platba proběhla
        }
        return false; // Málo peněz
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    // --- LOGIKA UPGRADŮ ---
    // Tyto metody budeš volat z tlačítek v Upgrade Menu
    public void UpgradeMaxHealth(int cost)
    {
        if (SpendMoney(cost))
        {
            maxHealth += 1;
            health = maxHealth; // Přidá život i k aktuálnímu zdraví
            UpdateUI();
        }
    }

    public void UpgradeMoveSpeed(int cost)
    {
        if (SpendMoney(cost))
        {
            moveSpeed += 0.5f;
            UpdateUI();
        }
    }

    public void UpgradeReloadTime(int cost)
    {
        if (SpendMoney(cost))
        {
            reloadTime -= 0.05f; // Sníží čas nabíjení = střílíš rychleji
            UpdateUI();
        }
    }

    // --- SYSTÉM ZDRAVÍ ---
    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (healthBar != null)
        {
            float pct = (float)health / Mathf.Max(1, maxHealth);
            healthBar.localScale = new Vector3(Mathf.Clamp01(pct), 1f, 1f);
        }
        if (moneyText != null)
            moneyText.text = money.ToString();
    }
}