using UnityEngine;

public class TurrentMovement: MonoBehaviour
{
    public GameObject TankShellPrefab;
    public Transform spawnPoint;
    public float shellSpeed = 10f;

      void Update()
    {
        Turn();
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void Turn()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
    public void Fire()
    {
        // vytvoří střelu na pozici a s rotací kanónu
        GameObject shell = Instantiate(TankShellPrefab, spawnPoint.position, spawnPoint.rotation);

        // vezme její rigidbody
        Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();

        // tady je důležité !!! ↓↓↓
        rb.linearVelocity = spawnPoint.up * shellSpeed;
    }
}
