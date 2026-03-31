using UnityEngine;

public class BossSpawner : MonoBehaviour
{
   public GameObject[] Prefabs;

    void Start()
    {
        SpawnRandomEnemy();
    }

    void SpawnRandomEnemy()
    {
        if (Prefabs.Length > 0)
        {

            int randomIndex = Random.Range(0, Prefabs.Length);
            
            Instantiate(Prefabs[randomIndex], transform.position, transform.rotation);
        }
    }
}
