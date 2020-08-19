using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MazePhysicalSpecification", menuName = "Specifications/MazePhysical")]
public class MazePhysicalSpecification : ScriptableObject
{
    [SerializeField] private float wallHeight;
    [SerializeField] private float wallThickness;
    [SerializeField] private float cellLength;

    public float WallHeight { get => wallHeight; }
    public float WallThickness { get => wallThickness; }
    public float CellSize { get => cellLength; }
    public float WallLength { get => cellLength - wallThickness; }
}
