using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Statistics")]
    public int maxHealth = 5;
    public int health = 5;
    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;
    public float damage = 1;
    public int money = 0;
    public float reloadTime = 0.75f;
    public float shellSpeed = 10f;

    [Header("UI References")]
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
    {
        Destroy(gameObject);
    }
    else {
        health = maxHealth;
        GameObject hbObj = GameObject.Find("HealthBarFill");
        if (hbObj != null) healthBar = hbObj.GetComponent<RectTransform>();

        GameObject mTextObj = GameObject.Find("MoneyField");
        if (mTextObj != null) moneyText = mTextObj.GetComponent<TextMeshProUGUI>();
        UpdateUI();
    }
        
    }

    void Start()
    {
        UpdateUI();
    }

    public bool SpendMoney(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void ResetHealth()
    {
        health = maxHealth;
        UpdateUI();
    }

    public void UpgradeMaxHealth(int cost)
    {
        if (SpendMoney(cost))
        {
            maxHealth += 1;
            health = maxHealth; 
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
            reloadTime -= 0.05f;
            UpdateUI();
        }
    }

    public void UpgradeDamage(int cost)
    {
        if (SpendMoney(cost))
        {
            damage += 0.5f;
            UpdateUI();
        }
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        UpdateUI();
        if (health <= 0)
        {
           if (LevelManager.Instance != null) LevelManager.Instance.Die();
        }
    }

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