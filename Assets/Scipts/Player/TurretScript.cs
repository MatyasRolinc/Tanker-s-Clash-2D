using UnityEngine;

public class TurrentScript : MonoBehaviour
{
    // kde se spawnují střely
    public GameObject TankShellPrefab;
    public Transform spawnPoint;

    // Reference se nyní napojí na globální staty
    private PlayerStats stats;

    // interní cooldown
    private float nextFireTime = 0f;

    void Start()
    {
        // --- ZMĚNA: Napojení na globální staty přes Singleton ---
        if (PlayerStats.instance != null)
        {
            stats = PlayerStats.instance;
        }
        else
        {
            Debug.LogWarning("TurretScript: PlayerStats.instance nebyl nalezen! Použijí se výchozí hodnoty.");
        }

        nextFireTime = 0f;
    }

    void Update()
    {
        Turn();
        Fire();
    }

    public void Turn()
    {
        if (Camera.main == null) return; // Ochrana proti chybě, pokud chybí kamera

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Úprava rotace -90f závisí na tom, jak máš otočený sprite věže
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    public void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Načtení vylepšených hodnot z PlayerStats
            float useReload = (stats != null) ? stats.reloadTime : 0.75f;
            float useShellSpeed = (stats != null) ? stats.shellSpeed : 10f;

            // Kontrola cooldownu
            if (Time.time < nextFireTime) return;

            if (TankShellPrefab != null && spawnPoint != null)
            {
                GameObject shell = Instantiate(TankShellPrefab, spawnPoint.position, spawnPoint.rotation);
                Rigidbody2D rbShell = shell.GetComponent<Rigidbody2D>();
                
                // Oprava: shell.layer nastavujeme přes ID vrstvy pro lepší výkon
                shell.layer = LayerMask.NameToLayer("Player");

                if (rbShell != null)
                {
                    // Použijeme rychlost střely ze statistik
                    rbShell.linearVelocity = spawnPoint.up * useShellSpeed;
                }
            }

            // Nastavení času pro další výstřel
            nextFireTime = Time.time + useReload;
        }
    }

    public float RemainingReloadTime()
    {
        return Mathf.Max(0f, nextFireTime - Time.time);
    }
}