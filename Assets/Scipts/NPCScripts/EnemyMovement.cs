using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;           // rychlost pohybu
    public float moveTime = 2f;            // jak dlouho pojede
    public float stopTime = 1f;            // jak dlouho stojí
    public float rotationSpeed = 180f; 
    public int health = 3;    // rychlost otáčení (stupně za sekundu)

    private Rigidbody2D rb;
    private bool isMoving = true;
    private float timer = 0f;
    private float targetAngle;
    private bool isTurning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // náhodná počáteční rotace
        targetAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isMoving && timer >= moveTime)
        {
            // Zastaví se a začne se otáčet
            isMoving = false;
            isTurning = true;
            timer = 0f;
            rb.linearVelocity = Vector2.zero;
            targetAngle = Random.Range(0f, 360f);
        }
        else if (!isMoving && !isTurning && timer >= stopTime)
        {
            // Po zastavení a otočení se znovu rozjede
            isMoving = true;
            timer = 0f;
        }

        // Plynulé otáčení během "isTurning"
        if (isTurning)
        {
            float currentAngle = transform.eulerAngles.z;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);

            // když se otočí, přestane se točit
            if (Mathf.Abs(Mathf.DeltaAngle(newAngle, targetAngle)) < 1f)
            {
                isTurning = false;
                timer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 forward = transform.up;
            rb.linearVelocity = forward * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TankShell"))
        {
            health -= 1;
            Destroy(collision.gameObject); // zničit projektil

            if (health <= 0)
            {
                // před zničením dej odměnu pokud má nepřítel komponentu EnemyReward
                var reward = GetComponent<EnemyReward>();
                if (reward != null)
                {
                    // předáváme projektil jako "killer" - EnemyReward si poradí a případně najde hráče
                    reward.GiveReward(collision.gameObject);
                }
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.EnemyKilled();
                }

                Destroy(gameObject);
            }
        }
    }
}
