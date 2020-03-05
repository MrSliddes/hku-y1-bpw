using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterNewMaze : MonoBehaviour
{
    /// <summary>
    /// The maze script, this gets assigned when this object gets created
    /// </summary>
    [HideInInspector] public MazeGeneration mazeGeneration;
       

    private void OnTriggerEnter(Collider other)
    {
        // If player collides with trigger generate a new maze
        if(other.tag == "Player")
        {
            // Trigger new generation
            mazeGeneration.generateRandom = true;
            mazeGeneration.GenerateMaze();
        }
    }
}
