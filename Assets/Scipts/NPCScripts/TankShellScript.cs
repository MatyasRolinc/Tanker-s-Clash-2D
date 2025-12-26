using UnityEngine;

public class TankShellScript : MonoBehaviour
{
    Rigidbody2D rb;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
