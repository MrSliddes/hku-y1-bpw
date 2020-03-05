using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public bool awayFromPlayer = true;
    public float spawnChance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn enemy
        if(Random.value < spawnChance)
        {
            if(awayFromPlayer)
            {
                if(Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) > 5)
                {
                    Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                }
            }
            else
            {
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
        }        
    }

    
}
