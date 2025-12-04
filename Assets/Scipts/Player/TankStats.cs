using UnityEngine;

public class TankStats : MonoBehaviour
{
    // základní statistiky tanku (upravit v Inspectoru)
    public int maxHP = 100;
    public int currentHP = 100;

    // pohyb
    public float speed = 5f;
    public float rotationSpeed = 150f;

    // útok
    public int damage = 10;
    public float reloadTime = 0.75f; // sekundy mezi střelami
    public float shellSpeed = 10f;

    // měna
    public int money = 0;

    void Reset()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }

    public void SetMaxHP(int newMax)
    {
        maxHP = Mathf.Max(1, newMax);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
    public void AddMoney(int amount)
    {
        money += amount;
    }
}