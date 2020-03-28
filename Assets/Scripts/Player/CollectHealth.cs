using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If player collides replenish health
/// </summary>
public class CollectHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().ReceiveHp();
            Destroy(gameObject);
        }
    }
}
