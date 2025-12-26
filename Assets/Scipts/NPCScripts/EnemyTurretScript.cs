using UnityEngine;

public class EnemyTurretScript : MonoBehaviour
{
    public Transform target;            // hráč (tank)
    public Transform firePoint;         // odkud vystřelí (pokud null => this.transform)
    public GameObject shellPrefab;
    public float shellSpeed = 8f;

    public float rotationSpeed = 120f;  // rychlost otáčení (°/s)
    public float angleOffset = 0f;      // ruční posun úhlu

    public float shootInterval = 5f;    // interval mezi výstřely (sekundy)
    private float nextFireTime = 0f;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
                target = player.transform;
        }

        if (firePoint == null) firePoint = transform;
        nextFireTime = Time.time + Random.Range(0f, shootInterval);
    }

    void Update()
    {
        if (!target) return;

            Vector2 dir = (Vector2)(target.position - transform.position);

            // tady přidáme offset
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;

            float newAngle = Mathf.MoveTowardsAngle(
                transform.eulerAngles.z,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            // kontrola přímé viditelnosti hráče (raycast) — pokud první hit je hráč => střílej
            Vector2 origin = firePoint.position;
            Vector2 toPlayer = (Vector2)(target.position - firePoint.position);
            float dist = toPlayer.magnitude;
        if (dist <= 0f) return;

        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, dist);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + shootInterval;
            }
        }
    }

    void Shoot()
    {
        if (shellPrefab == null || firePoint == null) return;

            GameObject shell = Instantiate(shellPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();
            shell.layer = LayerMask.NameToLayer("Enemy");
        if (rb != null)
            rb.linearVelocity = firePoint.up * shellSpeed;

        Destroy(shell, 8f);
    }
}
