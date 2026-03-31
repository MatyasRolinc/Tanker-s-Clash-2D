using UnityEngine;

public class BossBurstShell : MonoBehaviour
{
    [Header("Nastavení rozdělení")]
    public GameObject shellPrefab;    // Prefab střely, na které se to rozdělí (může to být ta samá)
    public int numberOfShards = 3;    // Na kolik kusů se rozdělí (3 nebo 4)
    public float spreadAngle = 30f;   // Celkový úhel rozptylu (vějíře)
    public float timeToSplit = 0.5f;  // Za jak dlouho se rozdělí

    public int damage = 1; // nastaví se při spawnování (věž, hráč apod.)

    private bool hasSplit = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(Split), timeToSplit);
    }

    void Split()
    {
        if (hasSplit) return;
        hasSplit = true;

        Vector2 baseDirection = transform.right;
        float speed = 0f;

        if (rb != null)
        {
            speed = rb.linearVelocity.magnitude;
            if (speed > 0.001f)
                baseDirection = rb.linearVelocity.normalized;
        }

        float angleStep = spreadAngle / (numberOfShards - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < numberOfShards; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Quaternion shardRotation = transform.rotation * Quaternion.Euler(0, 0, currentAngle);
            Vector2 shardDirection = Quaternion.Euler(0, 0, currentAngle) * baseDirection;

            GameObject shard = Instantiate(shellPrefab, transform.position, shardRotation);
            Rigidbody2D shardRb = shard.GetComponent<Rigidbody2D>();
            if (shardRb != null)
            {
                shardRb.linearVelocity = shardDirection * speed;
            }

            BossBurstShell script = shard.GetComponent<BossBurstShell>();
            if (script != null)
            {
                script.enabled = false; // Vypneme rozdělovací logiku na střepinách
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
