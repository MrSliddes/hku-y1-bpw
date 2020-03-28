using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When player collides with this add paper to score
/// </summary>
public class CollectPaper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().CollectedPaper();
            Destroy(gameObject);
        }
    }
}
