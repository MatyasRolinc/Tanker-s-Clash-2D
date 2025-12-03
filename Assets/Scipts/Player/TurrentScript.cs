using UnityEngine;

public class TurrentScript: MonoBehaviour
{
    public GameObject TankShellPrefab;
    public Transform spawnPoint;
    public float shellSpeed = 10f;

    // čas (sekundy) potřebný mezi dvěma výstřely (reload cooldown)
    public float reloadTime = 0.75f;

    // interní čas, kdy je možné znovu střílet
    private float nextFireTime = 0f;

    void Start()
    {
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
        // klik pro výstřel, ne držení; kontrola cooldownu
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time < nextFireTime)
                return; // ještě "reloaduje"

            GameObject shell = Instantiate(TankShellPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody2D rbShell = shell.GetComponent<Rigidbody2D>();
            if (rbShell != null)
            {
                rbShell.linearVelocity = spawnPoint.up * shellSpeed;
            }

            // nastavíme příští čas kdy lze znovu střílet
            nextFireTime = Time.time + reloadTime;
        }
    }

    // volitelné: zbývající čas reloadu (pro UI upgrade později)
    public float RemainingReloadTime()
    {
        return Mathf.Max(0f, nextFireTime - Time.time);
    }
}
