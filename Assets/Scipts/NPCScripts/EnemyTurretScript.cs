using UnityEngine;

public class EnemyTurretScript : MonoBehaviour
{
    public Transform target;            // hráč (tank)
    public float rotationSpeed = 120f;  // rychlost otáčení (°/s)
    public float angleOffset = 0f;      // ruční posun úhlu

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
                target = player.transform;
        }
    }

    void Update()
    {
        if (!target) return;

        Vector2 dir = target.position - transform.position;

        // tady přidáme offset
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;

        float newAngle = Mathf.MoveTowardsAngle(
            transform.eulerAngles.z,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
}
