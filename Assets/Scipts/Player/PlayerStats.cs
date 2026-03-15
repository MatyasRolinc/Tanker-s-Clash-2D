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
    public int damage = 1;
    public int money = 5000;
    public float reloadTime = 0.75f;
    public float shellSpeed = 10f;

    [Header("UI References")]
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Start()
    {
        health = maxHealth;
        
    }

    void Awake()
    {
        instance = this;

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
    public void ResetHealth()
    {
        health = maxHealth;
        
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

    public void UpgradeDamage(int cost)
    {
        if (SpendMoney(cost))
        {
            damage += 1;
            UpdateUI();
        }
    }

    // --- SYSTÉM ZDRAVÍ ---
    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        UpdateUI();
        if (health <= 0)
        {
           LevelManager.Instance.Die();
        }
    }

    // Wrapper used by other scripts (keeps naming short: Hit)
    public void Hit(int amount)
    {
        TakeDamage(amount);
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