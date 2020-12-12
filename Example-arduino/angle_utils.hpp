float signedAngleDifference(float a, float b)
{
  float dif = b - a;
  while (dif < -PI) dif += TWO_PI;
  while (dif > PI) dif -= TWO_PI;
  return dif;
}

float angleNormalize(float angle)
{
  while (angle < 0) angle += TWO_PI;
  while (angle >= TWO_PI) angle -= TWO_PI;
  return angle;
}

float rad2deg(float rad)
{
	return rad * 180 / PI;
}

float deg2rad(float deg)
{
	return deg * PI / 180;
}