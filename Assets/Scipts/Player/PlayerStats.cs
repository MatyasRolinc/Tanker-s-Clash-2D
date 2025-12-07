using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // základní staty (nastav v Inspectoru)
    public int maxHealth = 5;
    public int health;
    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;
    public int money = 0;

    // útok
    public float reloadTime = 0.75f;
    public float shellSpeed = 10f;

    // UI (volitelné - nastav v Inspectoru)
    public RectTransform healthBar;
    public TextMeshProUGUI moneyText;

    void Awake()
    {
        if (health <= 0) health = maxHealth;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateUI();
    }

    // finance
    public void AddMoney(int amount)
    {
        if (amount == 0) return;
        money += amount;
        UpdateUI();
    }

    public bool SpendMoney(int cost)
    {
        if (cost <= 0) return true;
        if (money >= cost)
        {
            money -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }

    // zdraví
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        UpdateUI();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateUI();
    }

    public void SetMaxHealth(int newMax)
    {
        maxHealth = Mathf.Max(1, newMax);
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateUI();
    }

    // jednoduché upgrade metody (vrací true pokud zaplaceno)
    public bool UpgradeMaxHealth(int cost, int addAmount)
    {
        if (!SpendMoney(cost)) return false;
        maxHealth += addAmount;
        health += addAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateUI();
        return true;
    }

    public bool UpgradeMoveSpeed(int cost, float addAmount)
    {
        if (!SpendMoney(cost)) return false;
        moveSpeed += addAmount;
        UpdateUI();
        return true;
    }


    // aktualizuje UI (pokud jsou pole nastavená)
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