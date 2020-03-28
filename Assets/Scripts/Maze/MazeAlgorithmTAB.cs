using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// My own rewritten maze algorithm
/// </summary>
public class MazeAlgorithmTAB : MazeAlgorithm
{
    private int currentX = 0;
    private int currentZ = 0;
    private bool mazeComplete = false;
    private MazeGeneration mazeGeneration;

    public MazeAlgorithmTAB(MazeCell[,] mazeCells) : base(mazeCells) { }

    public override void CreateMaze(MazeGeneration mazeGeneration)
    {
        mazeCells[currentX, currentZ].visited = true;
        this.mazeGeneration = mazeGeneration;

        // Create start object
        GameObject a = new GameObject();
        a.name = "Maze Start Point";
        a.transform.position = new Vector3(0, 1, 0);
        a.transform.SetParent(GameObject.FindObjectOfType<MazeGeneration>().mazeParent.transform);
        mazeGeneration.startPoint = a.transform;

        while (!mazeComplete)
        {
            CreateMazePath();
            FindEmptyCells();
        }

        Debug.Log("Finished maze paths");
    }

    /// <summary>
    /// Creates a maze path until no longer able to
    /// </summary>
    private void CreateMazePath()
    {
        bool pathStillAvailable = true;

        while(pathStillAvailable)
        {
            // Check if there is still a path
            bool north = false;
            bool east = false;
            bool south = false;
            bool west = false;

            AvailablePathDirections(currentX, currentZ, out north, out east, out south, out west);

            List<int> options = new List<int>();

            if (north) options.Add(0);
            if (east) options.Add(1);
            if (south) options.Add(2);
            if (west) options.Add(3);

            // Check if there are options
            if(options.Count == 0)
            {
                // No more paths
                pathStillAvailable = false;
                break;
            }
            else
            {
                // There are still options, pick a random one to continue the path
                int optionListNumber = Random.Range(0, options.Count);
                int dir = options[optionListNumber];

                switch (dir)
                {
                    case 0:
                        // Go north
                        DestroyWall(mazeCells[currentX, currentZ].wallNorth); // Destroy own wall north (cells are generated with 4 walls for each direction, in order to form a path from 2 cells you need to destroy 2 walls)
                        DestroyWall(mazeCells[currentX, currentZ + 1].wallSouth); // Destroy other south wall
                        currentZ++;
                        break;
                    case 1:
                        // Go east
                        DestroyWall(mazeCells[currentX, currentZ].wallEast); // Self
                        DestroyWall(mazeCells[currentX + 1, currentZ].wallWest); // Other
                        currentX++;
                        break;
                    case 2:
                        // Go south
                        DestroyWall(mazeCells[currentX, currentZ].wallSouth); // Self
                        DestroyWall(mazeCells[currentX, currentZ - 1].wallNorth); // Other
                        currentZ--;
                        break;
                    case 3:
                        // Go west
                        DestroyWall(mazeCells[currentX, currentZ].wallWest); // Self
                        DestroyWall(mazeCells[currentX - 1, currentZ].wallEast); // Other
                        currentX--;
                        break;
                    default: Debug.LogError("Error direction");
                        break;
                }

                // Visited cell
                mazeCells[currentX, currentZ].visited = true;
            }
        }
    }
    
    /// <summary>
    /// Checks if there are any empty cells left in the maze (and fills them if there are)
    /// </summary>
    private void FindEmptyCells()
    {
        for (int x = mazeX - 1; x >= 0; x--)
        {
            for (int z = mazeZ - 1; z >= 0; z--)
            {
                // Check for empty cell and if it has an adjacent cell
                if(!mazeCells[x, z].visited && CellHasAdjacentVisitedCell(x, z))
                {
                    currentX = x;
                    currentZ = z;
                    DestroyAdjacentWall(x, z);
                    mazeCells[x, z].visited = true;
                    return;
                }
            }
        }

        // No more empty cells, maze is complete
        mazeComplete = true;

        // Create 10 paper rolls to collect
        GameObject a;
        for (int i = 0; i < 11; i++)
        {
            a = mazeGeneration.CreateRoll();
            a.name = "Paper Roll";
            MazeGeneration mg = mazeGeneration;
            int posX = (int)Random.Range(0, mg.mazeX * 0.9f);
            int posZ = (int)Random.Range(0, mg.mazeZ * 0.9f);
            a.transform.position = new Vector3(posX * mg.size, 0, posZ * mg.size);
        }

        // Create 1 hp powerup
        a = mazeGeneration.CreatePowerupHp();        
        int hpPosX = (int)Random.Range(mazeGeneration.mazeX * 0.5f, mazeGeneration.mazeX * 0.9f);
        int hpPosZ = (int)Random.Range(mazeGeneration.mazeZ * 0.5f, mazeGeneration.mazeZ * 0.9f);
        a.transform.position = new Vector3(hpPosX * mazeGeneration.size, 0, hpPosZ * mazeGeneration.size);
    }

    /// <summary>
    /// Checks for available path directions from a point
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="north"></param>
    /// <param name="east"></param>
    /// <param name="south"></param>
    /// <param name="west"></param>
    private void AvailablePathDirections(int x, int z, out bool north, out bool east, out bool south, out bool west)
    {
        // Problem > its going backwards

        north = false;
        east = false;
        south = false;
        west = false;

        // Check north
        if (z < mazeZ - 1 && !mazeCells[x, z + 1].visited) north = true;

        // Check east
        if (x < mazeX - 1 && !mazeCells[x + 1, z].visited) east = true;

        // Check south
        if (z > 0 && !mazeCells[x, z - 1].visited) south = true;

        // Check west
        if (x > 0 && !mazeCells[x - 1, z].visited) west = true;        
    }

    /// <summary>
    /// Destroys a wall object if it exists
    /// </summary>
    /// <param name="wall"></param>
    void DestroyWall(GameObject wall)
    {
        if(wall == null)
        {
            Debug.LogWarning("No wall");
        }
        else
        {
            GameObject.Destroy(wall.gameObject);
        }
    }

    /// <summary>
    /// Destroy a cell wall adjacent to the cell (create a new path opening)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    private void DestroyAdjacentWall(int x, int z)
    {
        // Just go clockwise to check for a adjacent cell

        // Check north, east, south, west
        if(z < mazeZ - 1 && mazeCells[x, z + 1].visited)
        {
            DestroyWall(mazeCells[x, z].wallNorth); // Self
            DestroyWall(mazeCells[x, z + 1].wallSouth); // Other
        }
        else if(x < mazeX - 1 && mazeCells[x + 1, z].visited)
        {
            DestroyWall(mazeCells[currentX, currentZ].wallEast); // Self
            DestroyWall(mazeCells[currentX + 1, currentZ].wallWest); // Other
        }
        else if(z > 0 && mazeCells[x, z - 1].visited)
        {
            DestroyWall(mazeCells[currentX, currentZ].wallSouth); // Self
            DestroyWall(mazeCells[currentX, currentZ - 1].wallNorth); // Other
        }
        else if(x > 0 && mazeCells[x - 1, z].visited)
        {
            DestroyWall(mazeCells[currentX, currentZ].wallWest); // Self
            DestroyWall(mazeCells[currentX - 1, currentZ].wallEast); // Other
        }
        else
        {
            Debug.LogError("Error destroying adjacent wall");
        }
    }

    /// <summary>
    /// Check if a cell has a adjecent cell that has been visited (a connection)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private bool CellHasAdjacentVisitedCell(int x, int z)
    {
        int visitedCells = 0;

        // Look north
        if (z < mazeZ - 1 && mazeCells[x, z + 1].visited) visitedCells++;

        // Look east
        if (x < mazeX - 1 && mazeCells[x + 1, z].visited) visitedCells++;

        // Look south
        if (z > 0 && mazeCells[x, z - 1].visited) visitedCells++;

        // Look west
        if (x > 0 && mazeCells[x - 1, z].visited) visitedCells++;

        return visitedCells > 0;
    }

    
}
