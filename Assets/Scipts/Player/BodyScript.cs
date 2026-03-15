using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BodyScript : MonoBehaviour
{
    

    // Již nepotřebujeme nastavovat v Inspectoru, najdeme si to sami
    private PlayerStats stats;

    private Rigidbody2D rb;
    private float moveInput = 0f;
    private float rotateInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        // --- KLÍČOVÁ ZMĚNA: Napojení na globální statistiky ---
       stats = Object.FindFirstObjectByType<PlayerStats>();

       if (stats != null)
        {
            stats.AddMoney(0); // Aktualizuje UI na začátku
            stats.UpdateUI();
        }

       
        UpdateHealthBar();
        UpdateMoneyUI();
    }

    void Update()
    {
        moveInput = 0f;
        rotateInput = 0f;

        // Ovládání tanku (S/W pro pohyb, A/D pro rotaci)
        if (Input.GetKey(KeyCode.S)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.W)) moveInput = -1f;

        if (Input.GetKey(KeyCode.A)) rotateInput = 1f;
        else if (Input.GetKey(KeyCode.D)) rotateInput = -1f;
    }

    void FixedUpdate()
    {
        MoveTank(moveInput, rotateInput);
    }

    public void MoveTank(float moveValue, float rotateValue)
    {
        // Bereme hodnoty přímo z globálních statistik
        float speed = (stats != null) ? stats.moveSpeed : 5f;
        float rotSpeed = (stats != null) ? stats.rotationSpeed : 150f;

        Vector2 direction = transform.right * moveValue; 
        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        float newRot = rb.rotation + rotateValue * rotSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(newRot);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TankShell"))
        {
            // Pokud má střela svůj atribut damage, použijeme ho
            int dmg = 1;
            var shell = collision.gameObject.GetComponent<TankShellScript>();
            if (shell != null) dmg = shell.damage;

            Destroy(collision.gameObject);

            if (stats != null)
            {
                stats.TakeDamage(dmg);
                stats.UpdateUI();
            }

            UpdateHealthBar();

            if (stats != null && stats.health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Tyto metody nyní hlavně komunikují s globálními staty
    public void AddMoney(int amount)
    {
        if (stats != null)
            stats.AddMoney(amount);
        
        UpdateMoneyUI();
    }

    private void UpdateHealthBar() { if(stats != null) stats.UpdateUI(); }
    private void UpdateMoneyUI() { if(stats != null) stats.UpdateUI(); }

    public void didHit(GameObject itemShot)
    {
        AddMoney(1);
    }
}