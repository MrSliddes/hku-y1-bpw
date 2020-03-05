using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm
{
    protected MazeCell[,] mazeCells;
    protected int mazeX, mazeZ; // protected, it is accesable from this class MazeAlgorithm (the classes derived form this class)

    protected MazeAlgorithm(MazeCell[,] mazeCells) : base()
    {
        this.mazeCells = mazeCells;
        mazeX = mazeCells.GetLength(0);
        mazeZ = mazeCells.GetLength(1);
    }

    public abstract void CreateMaze(MazeGeneration mazeGeneration); // Other classes derived from this need this void
}
