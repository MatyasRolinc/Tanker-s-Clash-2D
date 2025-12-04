using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BodyScript : MonoBehaviour
{   
    public RectTransform healthBar;
    public TextMeshProUGUI moneyText;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;
    public int health = 5;
    public int maxHealth = 5;
    public int money = 0;
    
    private float moveInput = 0f;
    private float rotateInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        health = Mathf.Max(1, maxHealth);

        // inicializovat UI
        UpdateHealthBar();
        UpdateMoneyUI();
    }

    void Update()
    {
        // pouze čteme vstup zde
        moveInput = 0f;
        rotateInput = 0f;

        if (Input.GetKey(KeyCode.S))
            moveInput = 1f;
        else if (Input.GetKey(KeyCode.W))
            moveInput = -1f;

        if (Input.GetKey(KeyCode.A))
            rotateInput = 1f;
        else if (Input.GetKey(KeyCode.D))
            rotateInput = -1f;

        // Aktualizace UI každým framem (můžeš přesunout do místa kde se zdraví mění)
        UpdateHealthBar();
    }

    void FixedUpdate()
    {
        MoveTank(moveInput, rotateInput);
    }

    // veřejná metoda pro pohyb přes Rigidbody2D
    public void MoveTank(float moveValue, float rotateValue)
    {
        Vector2 direction = transform.right; // nebo transform.up podle modelu
        Vector2 newPos = rb.position + direction * moveValue * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        float newRot = rb.rotation + rotateValue * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(newRot);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TankShell"))
        {
            health -= 1;
            Destroy(collision.gameObject); // zničit projektil
            UpdateHealthBar();

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // aktualizuje škálování health baru (předpokládá pivot vlevo)
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        int safeMax = Mathf.Max(1, maxHealth);
        float healthPercent = Mathf.Clamp01((float)health / safeMax);
        healthBar.localScale = new Vector3(healthPercent, 1f, 1f);
    }

    // externí setter pro druhé skripty
    public void SetMaxHealth(int newMax)
    {
        maxHealth = Mathf.Max(1, newMax);
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateHealthBar();
    }

    // přidej tuto metodu dovnitř třídy BodyScript
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
        // volitelně: uložit persistentně
        // PlayerPrefs.SetInt("player_money", money);
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    // opravena pomocná metoda (pokud ji používáš)
    public void didHit(GameObject itemShot)
    {
        // když voláš tuto metodu, rozhodni kolik dát, tady příklad +1
        AddMoney(1);
    }
}


