float Vector2::magnitude() const
{
	return sqrt(x * x + y * y);
}

Vector2 Vector2::normalize() const
{
	return *this / magnitude();
}

float pointDistance(Vector2 a, Vector2 b)
{
	return (a - b).magnitude();
}

Vector2 operator-(const Vector2& lhs, const Vector2& rhs)
{
	Vector2 result;
	result.x = lhs.x - rhs.x;
	result.y = lhs.y - rhs.y;
	return result;
}

Vector2 operator+(const Vector2& lhs, const Vector2& rhs)
{
	Vector2 result;
	result.x = lhs.x + rhs.x;
	result.y = lhs.y + rhs.y;
	return result;
}

Vector2 operator/(const Vector2& lhs, float num)
{
	Vector2 result;
	result.x = lhs.x / num;
	result.y = lhs.y / num;
	return result;
}




bool operator==(const Vector2Int& lhs, const Vector2Int& rhs)
{
	return (lhs.x == rhs.x) && (lhs.y == rhs.y);
}

bool operator!=(const Vector2Int& lhs, const Vector2Int& rhs)
{
	return !operator==(lhs, rhs);
}


Direction oppositeDirection(Direction direction)
{
  switch(direction)
  {
	  case Direction::up:
		return Direction::down;
		break;
	  case Direction::right:
		return Direction::left;
		break;
	  case Direction::down:
		return Direction::up;
		break;
	  case Direction::left:
		return Direction::right;
		break;
	  default:
		return direction;
		break;
  }
}

Direction ccwDirection(Direction direction)
{
  switch(direction)
  {
	  case Direction::up:
		return Direction::left;
		break;
	  case Direction::right:
		return Direction::up;
		break;
	  case Direction::down:
		return Direction::right;
		break;
	  case Direction::left:
		return Direction::down;
		break;
	  default:
		return direction;
		break;
  }
}

Direction ccwDirection(Direction direction, int steps)
{
	if (steps < 0)
	{
		return cwDirection(direction, -steps);
	}
	steps %= 4;
	for (int i = 0; i < steps; ++i)
	{
		direction = ccwDirection(direction);
	}
	return direction;
}

Direction cwDirection(Direction direction)
{
  switch(direction)
  {
	  case Direction::up:
		return Direction::right;
		break;
	  case Direction::right:
		return Direction::down;
		break;
	  case Direction::down:
		return Direction::left;
		break;
	  case Direction::left:
		return Direction::up;
		break;
	  default:
		return direction;
		break;
  }
}

Direction cwDirection(Direction direction, int steps)
{
	if(steps < 0)
	{
		return ccwDirection(direction, -steps);
	}
	steps %= 4;
	for (int i = 0; i < steps; ++i)
	{
		direction = cwDirection(direction);
	}
	return direction;
}

int ccwStepDistance(Direction from, Direction to)
{
	int stepDistance = 0;
	while (from != to)
	{
		from = ccwDirection(from);
		stepDistance++;
	}
	return stepDistance;
}

int cwStepDistance(Direction from, Direction to)
{
	int stepDistance = 0;
	while (from != to)
	{
		from = cwDirection(from);
		stepDistance++;
	}
	return stepDistance;
}

Direction localToGlobalDirection(Direction localForward, Direction subject)
{
	int localToGlobalStepOffset = ccwStepDistance(localForward, Direction::up);
	Direction result = cwDirection(subject, localToGlobalStepOffset);
	return result;
}

Vector2Int Vector2Int::neighbor(Direction direction)
{
	Vector2Int result = *this;
	switch(direction)
	{
	  case Direction::up:
		result.y += 1;
		break;
	  case Direction::right:
		result.x += 1;
		break;
	  case Direction::down:
		result.y -= 1;
		break;
	  case Direction::left:
		result.x -= 1;
		break;
    }
	return result;
}

float Pose2::normalizedAngle() const
{
	return angleNormalize(angle);
}

Pose2 operator-(const Pose2& lhs, const Pose2& rhs)
{
	Pose2 result;
	result.position = lhs.position - rhs.position;
	result.angle = lhs.angle - rhs.angle;
	return result;
}
