using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BodyScript : MonoBehaviour
{
    public RectTransform healthBar;
    public TextMeshProUGUI moneyText;

    // všechny staty teď v PlayerStats (nastav v Inspectoru nebo se automaticky najde)
    public PlayerStats stats;

    private Rigidbody2D rb;
    private float moveInput = 0f;
    private float rotateInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (stats == null)
            stats = GetComponent<PlayerStats>() ?? GetComponentInParent<PlayerStats>();

        if (stats != null && stats.health <= 0)
            stats.health = stats.maxHealth;

        UpdateHealthBar();
        UpdateMoneyUI();
    }

    void Update()
    {
        // čtení vstupu (physics v FixedUpdate)
        moveInput = 0f;
        rotateInput = 0f;

        if (Input.GetKey(KeyCode.S)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.W)) moveInput = -1f;

        if (Input.GetKey(KeyCode.A)) rotateInput = 1f;
        else if (Input.GetKey(KeyCode.D)) rotateInput = -1f;
    }

    void FixedUpdate()
    {
        MoveTank(moveInput, rotateInput);
    }

    // pohyb používá hodnoty z stats
    public void MoveTank(float moveValue, float rotateValue)
    {
        float speed = (stats != null) ? stats.moveSpeed : 5f;
        float rotSpeed = (stats != null) ? stats.rotationSpeed : 150f;

        Vector2 direction = transform.right * moveValue; // nebo transform.up podle modelu
        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        float newRot = rb.rotation + rotateValue * rotSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(newRot);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TankShell"))
        {
            Destroy(collision.gameObject);

            if (stats != null)
                stats.TakeDamage(1);

            UpdateHealthBar();

            if (stats != null && stats.health <= 0)
                Destroy(gameObject);
        }
    }

    // Health bar založený na hodnotách v stats
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        int safeMax = Mathf.Max(1, (stats != null ? stats.maxHealth : 1));
        float current = (stats != null ? stats.health : safeMax);
        float healthPercent = Mathf.Clamp01(current / (float)safeMax);
        healthBar.localScale = new Vector3(healthPercent, 1f, 1f);
    }

    // přidání peněz přes PlayerStats
    public void AddMoney(int amount)
    {
        if (stats != null)
            stats.AddMoney(amount);

        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (moneyText == null) return;
        int display = (stats != null) ? stats.money : 0;
        moneyText.text = display.ToString();
    }

    // pomocná metoda kompatibilní s předchozím voláním
    public void didHit(GameObject itemShot)
    {
        AddMoney(1);
    }
}


