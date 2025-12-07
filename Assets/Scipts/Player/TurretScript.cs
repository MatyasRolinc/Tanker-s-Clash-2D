using UnityEngine;

public class TurrentScript : MonoBehaviour
{
    // kde se spawnují střely
    public GameObject TankShellPrefab;
    public Transform spawnPoint;

    // reference na PlayerStats (nastav v Inspectoru nebo najde parent)
    public PlayerStats stats;

    // interní cooldown
    private float nextFireTime = 0f;

    void Start()
    {
        if (stats == null)
            stats = GetComponentInParent<PlayerStats>();

        nextFireTime = 0f;
    }

    void Update()
    {
        Turn();
        Fire();
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
        if (Input.GetMouseButtonDown(0))
        {
            float useReload = (stats != null) ? stats.reloadTime : 0.75f;
            float useShellSpeed = (stats != null) ? stats.shellSpeed : 10f;

            if (Time.time < nextFireTime) return;

            if (TankShellPrefab != null && spawnPoint != null)
            {
                GameObject shell = Instantiate(TankShellPrefab, spawnPoint.position, spawnPoint.rotation);
                Rigidbody2D rbShell = shell.GetComponent<Rigidbody2D>();
                if (rbShell != null)
                    rbShell.linearVelocity = spawnPoint.up * useShellSpeed;
            }

            nextFireTime = Time.time + useReload;
        }
    }

    public float RemainingReloadTime()
    {
        return Mathf.Max(0f, nextFireTime - Time.time);
    }
}
