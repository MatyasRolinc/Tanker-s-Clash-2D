using UnityEngine;

public class BossShell : MonoBehaviour
{
    public GameObject smallBulletPrefab; // Menší střela, co vyletí ven
    public int bulletCount = 8;          // Počet střel v kruhu
    public float timeToSplit = 1f;       // Za jak dlouho se rozdělí
    public float smallBulletSpeed = 5f;

    void Start()
    {
        // Spustíme odpočet k rozdělení
        Invoke(nameof(Split), timeToSplit);
    }

    void Split()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // Výpočet směru pro každou malou střelu
            float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0f);
            Vector2 bulletDir = (bulletMoveVector - transform.position).normalized;

            // Vytvoření malé střely
            GameObject bullet = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);
            
            // Nastavení směru letu
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bulletDir * smallBulletSpeed;
            }

            angle += angleStep;
        }

        // Hlavní velká střela po rozdělení zmizí
        Destroy(gameObject);
    }
}