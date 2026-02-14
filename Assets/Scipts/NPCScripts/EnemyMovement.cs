using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDuration = 2f;
    public float stopDuration = 0.5f;
    public float rotationSpeed = 180f;
    public float health = 3f;

    public float obstacleCheckDistance = 2f;
    public LayerMask obstacleMask;

    private Rigidbody2D rb;
    private float timer;
    private bool isMoving = true;
    private float targetAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        PickNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isMoving)
        {
            if (IsObstacleAhead())
            {
                StopAndTurn();
                return;
            }

            if (timer >= moveDuration)
            {
                StopAndTurn();
            }
        }
        else
        {
            if (timer >= stopDuration)
            {
                PickNewDirection();
            }
        }

        RotateTowardsTarget();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = transform.up * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void StopAndTurn()
    {
        isMoving = false;
        timer = 0f;
        targetAngle = Random.Range(0f, 360f);
    }

    void PickNewDirection()
    {
        timer = 0f;
        isMoving = true;
        targetAngle = Random.Range(0f, 360f);
    }

    void RotateTowardsTarget()
    {
        float current = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(current, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
    
    bool IsObstacleAhead()
    {
        float radius = 0.4f;
        Vector2 origin = rb.position + (Vector2)transform.up * 0.2f;

        RaycastHit2D hit = Physics2D.CircleCast(
            origin,
            radius,
            transform.up,
            obstacleCheckDistance,
            obstacleMask
        );

        Debug.DrawRay(origin, transform.up * obstacleCheckDistance, Color.red);
        return hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kontrola, zda nás zasáhla střela
        if (collision.gameObject.CompareTag("TankShell"))
        {
            // Zničit střelu
            Destroy(collision.gameObject);

            // Snížit zdraví podle poškození hráče (pokud existuje), jinak použít 1
            int dmg = 1;
            if (PlayerStats.instance != null)
            {
                dmg = PlayerStats.instance.damage;
            }

            health -= dmg;
            Debug.Log($"Enemy hit by shell: -{dmg} HP (remaining {health})");

            if (health <= 0f)
            {
                Die();
            }
        }
    }

    void Die()
    {
        // 1. UDĚLENÍ ODMĚNY (Peníze do PlayerStats)
        EnemyReward reward = GetComponent<EnemyReward>();
        if (reward != null)
        {
            reward.GiveReward(); // Použije tvou novou přímočarou metodu
        }

        // 2. OZNÁMENÍ LEVEL MANAGERU (Pro přepnutí levelu)
        // Předpokládám, že LevelManager.Instance hlídá počet nepřátel v levelu
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.EnemyKilled();
        }

        // 3. ZNIČENÍ OBJEKTU
        Destroy(gameObject);
    }
}