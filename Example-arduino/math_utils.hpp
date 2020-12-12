Vector2 rayCast(Vector2 origin, float angle, float length)
{
	origin.x += length * cos(angle);
	origin.y += length * sin(angle);
	return origin;
}



bool isWithinRectangle(Vector2 point, Vector2 recSize)
{
	return point.x >= -recSize.x/2 && point.x <= recSize.x/2
		&& point.y >= -recSize.y/2 && point.y <= recSize.y/2;
}

bool isWithinRectangle(Vector2 point, Vector2 recCenter, Vector2 recSize)
{
	float forwardBoundary  = recCenter.y + recSize.y / 2;
	float rightBoundary    = recCenter.x + recSize.x / 2;
	float backwardBoundary = recCenter.y - recSize.y / 2;
	float leftBoundary     = recCenter.x - recSize.x / 2;
	
	return (point.x >= leftBoundary     && point.x < rightBoundary  )
		&& (point.y >= backwardBoundary && point.y < forwardBoundary);
}

bool isWithinCircle(Vector2 point, float radius)
{
	return point.magnitude() <= radius;
}

