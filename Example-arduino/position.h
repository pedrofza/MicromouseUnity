#ifndef POSITION_H
#define POSITION_H

#include "angle_utils.h"

struct Vector2Int;
struct Vector2;

enum class Direction
{
	up, right,
	down, left
};

Direction oppositeDirection(Direction direction);
Direction ccwDirection(Direction direction);
Direction ccwDirection(Direction direction, int steps);
Direction cwDirection(Direction direction);
Direction cwDirection(Direction direction, int steps);
Direction localToGlobalDirection(Direction localForward, Direction subject);
int ccwStepDistance(Direction from, Direction to);
int cwStepDistance(Direction from, Direction to);

struct Vector2
{
	float x;
	float y;
	float magnitude() const;
	Vector2 normalize() const;
};

Vector2 operator-(const Vector2& lhs, const Vector2& rhs);
Vector2 operator+(const Vector2& lhs, const Vector2& rhs);
Vector2 operator/(const Vector2& lhs, float num);

float pointDistance(Vector2 a, Vector2 b);

struct Vector2Int
{
	int x;
	int y;
	Vector2Int neighbor(Direction direction);
};

bool operator==(const Vector2Int& lhs, const Vector2Int& rhs);
bool operator!=(const Vector2Int& lhs, const Vector2Int& rhs);

struct Pose2
{
	Vector2 position;
	float angle;
	float normalizedAngle() const;
};

Pose2 operator-(const Pose2& lhs, const Pose2& rhs);


#include "position.hpp"

#endif
