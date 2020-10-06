using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LogicalMaze
{
    private LogicalCell[,] logicalCells;

    public LogicalMaze(int xSize, int ySize)
    {
        this.logicalCells = new LogicalCell[xSize, ySize];
    }

    public LogicalMaze(LogicalCell[,] cells)
    {
        this.logicalCells = cells;
    }

    public int xSize { get => logicalCells.GetLength(0); }
    public int ySize { get => logicalCells.GetLength(1); }
    
    public class LogicalCell
    {
        private static int leftWallBitPosition = 0;
        private static int rightWallBitPosition = 1;
        private static int topWallBitPosition = 2;
        private static int bottomWallBitPosition = 3;

        private uint walls;
        public bool LeftWall 
        {
            get => IsWallAtPosition(leftWallBitPosition);
            set => SetWallAtPosition(leftWallBitPosition, value);                       
        }
        
        public bool RightWall 
        {
            get => IsWallAtPosition(rightWallBitPosition);
            set => SetWallAtPosition(rightWallBitPosition, value);                       
        }

        public bool TopWall 
        {
            get => IsWallAtPosition(topWallBitPosition);
            set => SetWallAtPosition(topWallBitPosition, value);                       
        }

        public bool BottomWall 
        {
            get => IsWallAtPosition(bottomWallBitPosition);
            set => SetWallAtPosition(bottomWallBitPosition, value);                       
        }

        private bool IsWallAtPosition(int bitPosition)
        {
            return Convert.ToBoolean(walls & (1u << bitPosition));
        }

        private void SetWallAtPosition(int bitPosition, bool value)
        {
            uint mask = 1u << bitPosition;
            walls = (walls & ~mask) | (((value ? 1u : 0u) << bitPosition) & mask);
        }
    }
}
