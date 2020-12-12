#include "math_utils.h"
#include "maze_utils.h"

bool Cell::placeWall(Direction position)
{
	bool hadWall = hasWall(position);
	int index = wallPositionToIndex(position);
	m_walls |= (1 << index);
	return !hadWall;
}

bool Cell::removeWall(Direction position)
{
	bool hadWall = hasWall(position);
	int index = wallPositionToIndex(position);
	m_walls &= ~(1 << index);
	return hadWall;
}

bool Cell::hasWall(Direction position) const
{
	int index = wallPositionToIndex(position);
	return (1 << index) & m_walls;
}

//TODO: Handle invalid
int Cell::wallPositionToIndex(Direction position)
{
	switch(position)
	{
		case Direction::up:
			return 0;
			break;
		case Direction::right:
			return 1;
			break;
		case Direction::down:
			return 2;
			break;
		case Direction::left:
			return 3;
			break;
		default:
			return -1;
			break;
	}
}

template <int X, int Y, int N>
Maze<X, Y, N>::Maze()
{
}

template <int X, int Y, int N>
Cell& Maze<X, Y, N>::cellAt(Vector2Int position)
{
	int x = position.x;
	int y = position.y;
	return m_cells[x][y];
}

template <int X, int Y, int N>
bool Maze<X, Y, N>::isWithinBounds(Vector2Int position) const
{
	return position.x >= 0 && position.x < xSize() &&
		   position.y >= 0 && position.y < ySize();
}

template <int X, int Y, int N>
bool Maze<X, Y, N>::hasNeighbor(Vector2Int cellPosition, Direction direction) const
{
	Vector2Int neighborPosition = cellPosition.neighbor(direction);
	return isWithinBounds(neighborPosition);
}

template <int X, int Y, int N>
const Cell& Maze<X, Y, N>::cellAt(Vector2Int position) const
{
	int x = position.x;
	int y = position.y;
	return m_cells[x][y];
}

template <int X, int Y, int N>
int Maze<X, Y, N>::xSize() const
{
	return X;
}

template <int X, int Y, int N>
int Maze<X, Y, N>::ySize() const
{
	return Y;
}

template <int X, int Y, int N>
bool Maze<X, Y, N>::placeWall(Vector2Int cellPosition, Direction wallPosition)
{
	if (!isWithinBounds(cellPosition)) return false;
	Cell& targetCell = cellAt(cellPosition);
	bool targetCellSuccessFlag = targetCell.placeWall(wallPosition);
	if (!hasNeighbor(cellPosition, wallPosition))
	{
		return targetCellSuccessFlag;
	}
	Vector2Int neighborPosition = cellPosition.neighbor(wallPosition);
	Cell& neighborCell = cellAt(neighborPosition);
	Direction neighborWallPosition = oppositeDirection(wallPosition);
	bool neighborCellSuccessFlag = neighborCell.placeWall(neighborWallPosition);
	return targetCellSuccessFlag && neighborCellSuccessFlag;
}

template <int X, int Y, int N>
bool Maze<X, Y, N>::removeWall(Vector2Int cellPosition, Direction wallPosition)
{
	if (!isWithinBounds(cellPosition)) return false;
	Cell& targetCell = cellAt(cellPosition);
	bool targetCellSuccessFlag = targetCell.removeWall(wallPosition);
	if (!hasNeighbor(cellPosition, wallPosition))
	{
		return targetCellSuccessFlag;
	}
	Vector2Int neighborPosition = cellPosition.neighbor(wallPosition);
	Cell& neighborCell = cellAt(neighborPosition);
	Direction neighborWallPosition = oppositeDirection(wallPosition);
	bool neighborCellSuccessFlag = neighborCell.removeWall(neighborWallPosition);
	return targetCellSuccessFlag && neighborCellSuccessFlag;
}

template <int X, int Y, int N>
bool Maze<X, Y, N>::hasWall(Vector2Int cellPosition, Direction wallPosition) const
{
	if (!isWithinBounds(cellPosition)) return false;
	const Cell& targetCell = cellAt(cellPosition);
	bool targetCellSuccessFlag = targetCell.hasWall(wallPosition);
	if (!hasNeighbor(cellPosition, wallPosition))
	{
		return targetCellSuccessFlag;
	}
	Vector2Int neighborPosition = cellPosition.neighbor(wallPosition);
	const Cell& neighborCell = cellAt(neighborPosition);
	Direction neighborWallPosition = oppositeDirection(wallPosition);
	bool neighborCellSuccessFlag = neighborCell.hasWall(neighborWallPosition);
	return targetCellSuccessFlag && neighborCellSuccessFlag;
}

template <int X, int Y, int N>
Vector2 Maze<X, Y, N>::physicalCellPosition(Vector2Int position) const
{
	Vector2 result;
	result.x = position.x * m_cellSize.x;
	result.y = position.y * m_cellSize.y;
	return result;
}

template <int X, int Y, int N>
Vector2 Maze<X, Y, N>::physicalWallPosition(Vector2Int cellPosition, Direction wallPosition) const
{
  Vector2 pcp = physicalCellPosition(cellPosition);
	Vector2 physicalCellPosition2 = pcp;
	Vector2 wallOffset;
	switch(wallPosition)
	{
		case Direction::up:
			wallOffset = {0, m_cellSize.y / 2};
			break;
		case Direction::right:
			wallOffset = {m_cellSize.x / 2, 0};
			break;
		case Direction::down:
			wallOffset = {0, -m_cellSize.y / 2};
			break;
		case Direction::left:
			wallOffset = {-m_cellSize.x / 2, 0};
			break;
		default:
			wallOffset = {0, 0};
			break;
	}
	Vector2 result = physicalCellPosition2 + wallOffset;
	return result;
}

template <int X, int Y, int N>
void Maze<X, Y, N>::setCellSize(Vector2 cellSize)
{
	m_cellSize = cellSize;
}

template <int X, int Y, int N>
Vector2Int Maze<X, Y, N>::homePosition() const
{
	return m_home;
}

template <int X, int Y, int N>
void Maze<X, Y, N>::setHomePosition(Vector2Int home)
{
	m_home = home;
}

template <int X, int Y, int N>
etl::array_view<const Vector2Int> Maze<X, Y, N>::goalPositions() const
{
	return etl::array_view<const Vector2Int>(m_goals);
}

template <int X, int Y, int N>
void Maze<X, Y, N>::addGoalPosition(Vector2Int goal)
{
	m_goals.push_back(goal);
}

template <int X, int Y, int N>
bool Maze<X, Y, N>::pointToWall(Vector2 point, float allowDistance, float disallowDistance, Vector2Int& cell, Direction& wall, float& wallDistance) const
{
	//TODO: assert
	if (allowDistance > disallowDistance)
	{
		allowDistance = disallowDistance;
	}
	
	cell = pointToCellCenter(point);
	
	Vector2 recSize = m_cellSize;
	recSize.x -= 2*allowDistance;
	recSize.y -= 2*allowDistance;

	bool allowTestPass = !isWithinRectangle(point, physicalCellPosition(cell) , recSize);
	if(!allowTestPass)
	{
		return false;
	}
	
	etl::array<Vector2, 4> corners {cellCorners(cell)};
	for (const Vector2& corner : corners)
	{
		if (pointDistance(corner, point) <= disallowDistance)
		{
			return false;
		}
		
	}
	
	Vector2 forwardEdge  {physicalWallPosition(cell, Direction::up)};
	Vector2 rightEdge    {physicalWallPosition(cell, Direction::right)};
	Vector2 backwardEdge {physicalWallPosition(cell, Direction::down)};
	Vector2 leftEdge     {physicalWallPosition(cell, Direction::left)};
	
	float forwardEdgeDistance = pointDistance(point, forwardEdge);
	float rightEdgeDistance = pointDistance(point, rightEdge);
	float backwardEdgeDistance = pointDistance(point, backwardEdge);
	float leftEdgeDistance=  pointDistance(point, leftEdge);

	float* minDistance = etl::multimin(&forwardEdgeDistance, &rightEdgeDistance, &backwardEdgeDistance, &leftEdgeDistance);
	if (minDistance == &forwardEdgeDistance)  wall = Direction::up;
	if (minDistance == &rightEdgeDistance)    wall = Direction::right;
	if (minDistance == &backwardEdgeDistance) wall = Direction::down;
	if (minDistance == &leftEdgeDistance)     wall = Direction::left;
	wallDistance = *minDistance;

	return true;
}

template <int X, int Y, int N>
Vector2Int Maze<X, Y, N>::pointToCellCenter(Vector2 point) const
{
	Vector2Int center;
	center.x = round(point.x / m_cellSize.x);
	center.y = round(point.y / m_cellSize.y);
	return center;
}

template <int X, int Y, int N>
etl::array<Vector2, 4> Maze<X, Y, N>::cellCorners(Vector2Int cell) const
{
	Vector2 c = physicalCellPosition(cell);
	etl::array<Vector2, 4> arr;
	
	arr[0] = c + Vector2{+m_cellSize.x / 2, +m_cellSize.y / 2};
	arr[1] = c + Vector2{+m_cellSize.x / 2, -m_cellSize.y / 2};
	arr[2] = c + Vector2{-m_cellSize.x / 2, +m_cellSize.y / 2};
	arr[3] = c + Vector2{-m_cellSize.x / 2, -m_cellSize.y / 2};
	return arr;
}

	
