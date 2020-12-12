

Maze<16, 16, 4> classicMicromouseMaze()
{
	Maze<16, 16, 4> maze;
	maze.setCellSize(CLASSIC_MICROMOUSE_MAZE_SIZE);
	surroundMazeWithWalls(maze);
	maze.placeWall({0, 0}, Direction::right);
	maze.setHomePosition({0, 0});
	maze.addGoalPosition({7, 7});
	maze.addGoalPosition({7, 8});
	maze.addGoalPosition({8, 7});
	maze.addGoalPosition({8, 8});
	return maze;
}

template <int X, int Y, int N>
void surroundMazeWithWalls(Maze<X, Y, N>& maze)
{
  for (auto i = 0; i < maze.xSize(); ++i)
  {
    for (auto j = 0; j < maze.ySize(); ++j)
    {
      Vector2Int cellPosition = {i, j};
      if (i == 0)
      {
        maze.placeWall(cellPosition, Direction::left);
      }
      else if (i == maze.xSize() - 1)
      {
        maze.placeWall(cellPosition, Direction::right);
      }
      if (j == 0)
      {
        maze.placeWall(cellPosition, Direction::down);
      }
      else if (j == maze.ySize() - 1)
      {
        maze.placeWall(cellPosition, Direction::up);
      }
    }
  }
}


