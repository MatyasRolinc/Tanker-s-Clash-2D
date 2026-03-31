using UnityEngine;

public class TankShellScript : MonoBehaviour
{
    Rigidbody2D rb;
    public int damage = 1; // nastaví se při spawnování (věž, hráč apod.)
    public void OnCollisionEnter2D(Collision2D collision)
    {
    Destroy(gameObject);
    }
}
