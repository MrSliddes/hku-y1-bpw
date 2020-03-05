using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGeneration : MonoBehaviour
{
    // Tutorial used for this stuff: https://www.youtube.com/watch?v=IrO4mswO2o4

    /// <summary>
    /// The x width and z depth of the maze
    /// </summary>
    public int mazeX, mazeZ; // Rows are the x axis, colums are the z axis (in 3D)
    /// <summary>
    /// The floor of the maze
    /// </summary>
    public GameObject mazeFloor;
    /// <summary>
    /// The wall of the maze
    /// </summary>
    public GameObject mazeWall;
    /// <summary>
    /// The parent all maze objects are parented to (for easy destroying)
    /// </summary>
    [HideInInspector] public GameObject mazeParent;
    /// <summary>
    /// The object the player has to reach to enter a new maze
    /// </summary>
    public GameObject EnterNewMaze;
    /// <summary>
    /// Size of 1 maze cell
    /// </summary>
    public float size = 4f;

    [Header("Generation values")]
    /// <summary>
    /// True = Generates the maze fully, False = Generates only the cells without pathfinding in the maze
    /// </summary>
    public bool generateFully = true;
    /// <summary>
    /// True = Generate a new maze
    /// </summary>
    public bool generateNew = false;
    [Range(0, 20000000)]
    public int mazeSeed = 0;
    /// <summary>
    /// True = When maze gets generated use a random seed
    /// </summary>
    public bool generateRandom = false;

    private MazeCell[,] mazeCells;
    private NavMeshSurface navMeshSurface;

    [HideInInspector] public Transform startPoint, finishPoint;    

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        navMeshSurface = GetComponent<NavMeshSurface>();

        // Initialize the maze
        GenerateMaze();        
    }

    // Update is called once per frame
    void Update()
    {
        // Regenerate new
        if(generateNew)
        {
            GenerateMaze();
            generateNew = false;
        }
    }

    private void InitializeMaze()
    {
        // Editor check
        if(mazeX < 1 || mazeZ < 1)
        {
            Debug.LogError("MazeGeneration::ERROR::Value of mazeRows/mazeColumns is too low. Please assign a higher number in the editor.");
            return;
        }

        // Destroy parent if it exists
        if(mazeParent != null)
        {
            Destroy(mazeParent);
        }
        mazeParent = new GameObject();
        mazeParent.name = "Maze parent";

        mazeCells = new MazeCell[mazeX, mazeZ];

        for (int r = 0; r < mazeX; r++)
        {
            for (int c = 0; c < mazeZ; c++)
            {
                mazeCells[r, c] = new MazeCell();
                MazeCell mc = mazeCells[r, c];

                // Create floor
                mc.floor = Instantiate(mazeFloor, new Vector3(r * size, 0, c * size), Quaternion.identity) as GameObject;
                mc.floor.name = "Floor " + r + ", " + c;
                mc.floor.transform.SetParent(mazeParent.transform);

                // Create walls
                // Reminder: The wall default rotation is 0 facing towords the positive z axis
                // Row is x axis and Column is z axis

                // Create north wall                
                mc.wallNorth = Instantiate(mazeWall, new Vector3(r * size, 0, c * size), Quaternion.identity) as GameObject;
                mc.wallNorth.name = "Wall North " + r + ", " + c;
                // No rotation needed
                mc.wallNorth.transform.SetParent(mc.floor.transform);
                
                // Create east wall
                mc.wallEast = Instantiate(mazeWall, new Vector3(r * size, 0, c * size), Quaternion.identity) as GameObject;
                mc.wallEast.name = "Wall East " + r + ", " + c;
                mc.wallEast.transform.localEulerAngles = new Vector3(0, 90, 0);
                mc.wallEast.transform.SetParent(mc.floor.transform);

                // Create south wall
                mc.wallSouth = Instantiate(mazeWall, new Vector3(r * size, 0, c * size), Quaternion.identity) as GameObject;
                mc.wallSouth.name = "Wall South " + r + ", " + c;
                mc.wallSouth.transform.localEulerAngles = new Vector3(0, 180, 0);
                mc.wallSouth.transform.SetParent(mc.floor.transform);

                // Create west wall                
                mc.wallWest = Instantiate(mazeWall, new Vector3(r * size, 0, c * size), Quaternion.identity) as GameObject;
                mc.wallWest.name = "Wall West " + r + ", " + c;
                mc.wallWest.transform.localEulerAngles = new Vector3(0, -90, 0);
                mc.wallWest.transform.SetParent(mc.floor.transform);
                
            }
        }
    }

    public void GenerateMaze()
    {
        if(generateRandom)
        {
            mazeSeed = Random.Range(0, 20000000);
        }
        Random.InitState(mazeSeed);

        InitializeMaze();

        // Use the maze algorithm
        if (generateFully)
        {
            MazeAlgorithm ma = new MazeAlgorithmTAB(mazeCells);
            ma.CreateMaze(this);
        }

        Debug.Log("Maze cells: " + mazeCells.Length);

        // Set player to start point and generate new Enter point to new maze for player
        GameObject.FindWithTag("Player").transform.position = startPoint.transform.position;
        GameObject a = Instantiate(EnterNewMaze, finishPoint.transform.position, Quaternion.identity) as GameObject;
        a.transform.SetParent(mazeParent.transform);
        a.GetComponent<EnterNewMaze>().mazeGeneration = this;

        StopCoroutine(BuildNavMeshSurface());
        StartCoroutine(BuildNavMeshSurface());
    }

    IEnumerator BuildNavMeshSurface()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        navMeshSurface.BuildNavMesh();
        Debug.Log("Maze Finished");
        yield break;
    }
}
