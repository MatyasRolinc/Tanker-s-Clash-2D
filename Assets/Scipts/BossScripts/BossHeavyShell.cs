using UnityEngine;

public class BossShell : MonoBehaviour
{
    public GameObject smallBulletPrefab;
    public int bulletCount = 8;
    public float timeToSplit = 1f;
    public float smallBulletSpeed = 5f;

    void Start()
    {
        Invoke(nameof(Split), timeToSplit);
    }

    void Split()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0f);
            Vector2 bulletDir = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bulletDir * smallBulletSpeed;
            }

            angle += angleStep;
        }

        Destroy(gameObject);
    }
}