#ifndef MAZE_H
#define MAZE_H

#include "position.h"
#include "etl/vector.h"
#include "etl/array_view.h"
#include "etl/array.h"

class Cell
{
	private:
	uint8_t m_walls;

	public:
	static int wallPositionToIndex(Direction position);
	
	bool placeWall(Direction position);
	bool removeWall(Direction position);
	bool hasWall(Direction position) const;
};

class IMaze
{
	public:
	virtual int xSize() const = 0;
	virtual int ySize() const = 0;
	virtual bool hasWall(Vector2Int cellPosition, Direction wallPosition) const = 0;
	virtual Vector2Int homePosition() const = 0;
	virtual etl::array_view<const Vector2Int> goalPositions() const = 0;
	virtual Vector2 physicalCellPosition(Vector2Int position) const = 0;
	virtual Vector2 physicalWallPosition(Vector2Int position, Direction wallPosition) const = 0;

};

template <int X, int Y, int N>
class Maze : public IMaze
{
	private:
	Vector2 m_cellSize;
	Cell m_cells[X][Y];
	Vector2Int m_home;
	etl::vector<Vector2Int, N> m_goals;
	const Cell& cellAt(Vector2Int position) const;
	      Cell& cellAt(Vector2Int position);
	bool isWithinBounds(Vector2Int position) const;
	bool hasNeighbor(Vector2Int cellPosition, Direction direction) const;
	
	public:
	Maze();
    int xSize() const override;
	int ySize() const override;
	bool placeWall(Vector2Int cellPosition, Direction wallPosition);
	bool removeWall(Vector2Int cellPosition, Direction wallPosition);
	bool hasWall(Vector2Int cellPosition, Direction wallPosition) const override;
	Vector2 physicalCellPosition(Vector2Int position) const override;
	Vector2 physicalWallPosition(Vector2Int position, Direction wallPosition) const override;
	
	bool pointToWall(Vector2 point, float allowDistance, float disallowDistance, Vector2Int& cell, Direction& wall, float& distanceToWall) const;
	Vector2Int pointToCellCenter(Vector2 point) const;
	etl::array<Vector2, 4> cellCorners(Vector2Int cell) const;
	
	void setCellSize(Vector2 cellSize);
	void setHomePosition(Vector2Int home);
	void addGoalPosition(Vector2Int goal);
	
	Vector2Int homePosition() const override;
	etl::array_view<const Vector2Int> goalPositions() const override;
};

#include "maze.hpp"

#endif
