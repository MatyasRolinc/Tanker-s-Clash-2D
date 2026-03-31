using UnityEngine;


public class EnemyDualFireScript : MonoBehaviour
{
private Transform target;
public Transform firePointLeft;
public Transform firePointRight;
public GameObject shellPrefab;
public GameObject muzzleFlashPrefab; 
public float muzzleFlashDuration = 0.6f;
public int damage = 1; 
public float shellSpeed = 8f;

    public float rotationSpeed = 120f; 
    public float angleOffset = 0f;      

    public float shootInterval = 5f;    
    private float nextFireTime = 0f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
            target = player.transform;

        if (firePointLeft == null) firePointLeft = transform;
        if (firePointRight == null) firePointRight = transform;
        nextFireTime = Time.time + Random.Range(0f, shootInterval);
    }

    void Update()
    {
        if (!target) return;

            Vector2 dir = (Vector2)(target.position - transform.position);

            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;

            float newAngle = Mathf.MoveTowardsAngle(
                transform.eulerAngles.z,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            bool canSee = false;
            // levý
            Vector2 originL = firePointLeft.position;
            Vector2 toPlayerL = (Vector2)(target.position - firePointLeft.position);
            float distL = toPlayerL.magnitude;
            if (distL > 0f)
            {
                RaycastHit2D hitL = Physics2D.Raycast(originL, toPlayerL.normalized, distL);
                if (hitL.collider != null && hitL.collider.CompareTag("Player")) canSee = true;
            }
            // pravý
            Vector2 originR = firePointRight.position;
            Vector2 toPlayerR = (Vector2)(target.position - firePointRight.position);
            float distR = toPlayerR.magnitude;
            if (distR > 0f)
            {
                RaycastHit2D hitR = Physics2D.Raycast(originR, toPlayerR.normalized, distR);
                if (hitR.collider != null && hitR.collider.CompareTag("Player")) canSee = true;
            }

            if (canSee)
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
        if (shellPrefab == null || firePointLeft == null || firePointRight == null) return;

        // Levá střela
        GameObject shellL = Instantiate(shellPrefab, firePointLeft.position, firePointLeft.rotation);
        var shellScriptL = shellL.GetComponent<TankShellScript>();
        if (shellScriptL != null) shellScriptL.damage = damage;
        Rigidbody2D rbL = shellL.GetComponent<Rigidbody2D>();
        shellL.layer = LayerMask.NameToLayer("Enemy");
        if (rbL != null)
            rbL.linearVelocity = firePointLeft.up * shellSpeed;
        Destroy(shellL, 8f);

        // Pravá střela
        GameObject shellR = Instantiate(shellPrefab, firePointRight.position, firePointRight.rotation);
        var shellScriptR = shellR.GetComponent<TankShellScript>();
        if (shellScriptR != null) shellScriptR.damage = damage;
        Rigidbody2D rbR = shellR.GetComponent<Rigidbody2D>();
        shellR.layer = LayerMask.NameToLayer("Enemy");
        if (rbR != null)
            rbR.linearVelocity = firePointRight.up * shellSpeed;
        Destroy(shellR, 8f);

        PlayFireEffects();
    }

    void PlayFireEffects()
    {
        PlayMuzzleFlash(firePointLeft);
        PlayMuzzleFlash(firePointRight);
    }

    void PlayMuzzleFlash(Transform firePoint)
    {
        if (muzzleFlashPrefab == null || firePoint == null) return;

        GameObject fx = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
        Destroy(fx, muzzleFlashDuration);
    }
}

