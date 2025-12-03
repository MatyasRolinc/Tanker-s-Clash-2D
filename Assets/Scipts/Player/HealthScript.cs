using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BodyScript tank;
    RectTransform rectTrans;
    void Start()
    {
       rectTrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float healthPercent = (float)tank.health / tank.maxHealth;
        rectTrans.localScale = new Vector3(healthPercent, 1, 1);
    }
}
