using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BodyScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;
    public int health = 5;
    public int maxHealth = 5;

    private Rigidbody2D rb;
    private float moveInput = 0f;
    private float rotateInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        health = maxHealth;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TankShell"))
        {
            health -= 1;
            Destroy(collision.gameObject); // zničit projektil
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}


