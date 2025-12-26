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
    public TankShellScript shell;

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
            // když narazí na překážku → hned zastaví a otočí se
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
            // čekání
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
    float radius = 0.4f; // cca polovina šířky tanku
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
        Debug.Log("Enemy hit by: " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag("TankShell"))
            return;

        // zničit projektil
        Destroy(collision.gameObject);

        // snížit zdraví
        health -= 1;

        if (health <= 0)
        {
            // udělit odměnu pokud je komponenta přítomná
            var reward = GetComponent<EnemyReward>();
            if (reward != null)
                reward.GiveReward(collision.gameObject);

            // oznámit LevelManageru, že nepřítel zemřel
            if (LevelManager.Instance != null)
                LevelManager.Instance.EnemyKilled();


            Destroy(gameObject);
        }
    }
}
