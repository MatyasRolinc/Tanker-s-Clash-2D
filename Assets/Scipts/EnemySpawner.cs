using UnityEngine;

public class BossSpawner : MonoBehaviour
{
   public GameObject[] Prefabs; // Tady v Inspektoru nastavíš velikost 2 a přetáhneš tam oba bossy

    void Start()
    {
        SpawnRandomBoss();
    }

    void SpawnRandomBoss()
    {
        if (Prefabs.Length > 0)
        {
            // Vybere náhodné číslo 0 nebo 1
            int randomIndex = Random.Range(0, Prefabs.Length);
            
            // Vytvoří vybraného bosse na pozici tohoto spawneru
            Instantiate(Prefabs[randomIndex], transform.position, transform.rotation);
        }
    }
}
