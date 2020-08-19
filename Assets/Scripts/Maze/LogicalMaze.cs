using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalMaze : ScriptableObject
{
    [SerializeField] private int xSize;
    
    [SerializeField] private int ySize;
    
    [SerializeField] private Vector2Int homeCell;
    
    [SerializeField] private Vector2Int[] goalCells;

    // private MazePhysicalSpecifcation specs;
    // public LogicMaze(MazePhysicalSpecifcation specifcation)
    // {
    //     this.specs = specifcation;
    // }

}
