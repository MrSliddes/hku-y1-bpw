using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    /// <summary>
    /// Has this cell been visited by the algorithm?
    /// </summary>
    public bool visited = false;
    /// <summary>
    /// The values the maze objects can have
    /// </summary>
    public GameObject wallNorth, wallSouth, wallEast, wallWest, floor;
}

public enum WallType
{
    north,
    east,
    south,
    west
}