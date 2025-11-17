using UnityEngine;

public class TurrentMovement: MonoBehaviour
{
    public GameObject TankShellPrefab;
    public Transform spawnPoint;

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
        if (Input.GetButtonDown("Fire1"))
        {
           GameObject bullet = Instantiate(TankShellPrefab, spawnPoint.position, transform.rotation);
        }
    }
}
