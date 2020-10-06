using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MazeMetadata
{
    public string Name { get; private set; }
}

[Serializable]
public class MazeEntry
{
    private MazeMetadata metadata;
    private LogicalMaze logicalMaze;
    private Vector2Int home;
    private Vector2Int[] goals;
}
