using UnityEngine;

public class TankShellScript : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
